using Invynite.Domain.Entities.Inven;
using Invynite.Domain.Enums;
using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.Inventories.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Services.Inventories;

public class InventoryAdjustmentService(AppDbContext context) : IInventoryAdjustmentService
{
    private readonly AppDbContext context = context;

    public async Task<CreateInventoryAdjustmentResponse> CreateAdjustmentAsync(CreateInventoryAdjustmentRequest request)
    {
        var materialToAdjust = await context.Inventories
            .Where(i =>
                i.WarehouseId == request.WarehouseId &&
                i.MaterialId != null &&
                i.MaterialId == request.MaterialId)
            .FirstOrDefaultAsync();

        if (materialToAdjust == null)
            throw new NotFoundException($"Material with id: {request.MaterialId} does not exist at warehouse with id: {request.WarehouseId}");

        decimal prevQty = materialToAdjust.Quantity;

        decimal newQty = request.NewQuantity;

        decimal differ = Math.Abs(prevQty - newQty);

        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var adjustment = new InventoryAdjustment
            {
                WarehouseId = request.WarehouseId,
                MaterialId = request.MaterialId,
                PreviousQuantity = prevQty,
                NewQuantity = newQty,
                Difference = differ,
                Reason = request.Reason
            };

            await context.InventoryAdjustments.AddAsync(adjustment);

            materialToAdjust.Quantity = newQty;

            await context.SaveChangesAsync();

            var movement = new StockMovement
            {
                WarehouseId = request.WarehouseId,
                MaterialId = request.MaterialId,
                Quantity = differ,
                MovementType = prevQty > newQty ? MovementType.OUT : MovementType.IN,
                ReferenceId = adjustment.Id,
                CreatedAt = DateTime.UtcNow
            };

            await context.StockMovements.AddAsync(movement);

            await context.SaveChangesAsync();

            var result = await context.InventoryAdjustments
                .Where(a => a.Id == adjustment.Id)
                .Select(a => new CreateInventoryAdjustmentResponse(
                    a.Warehouse.Name,
                    a.Material.Name,
                    a.NewQuantity,
                    a.Reason,
                    a.CreatedAt))
                .FirstOrDefaultAsync();

            await transaction.CommitAsync();

            return result!;
        }
        catch
        {
            await transaction.RollbackAsync();

            throw;
        }
    }

    public async Task<IEnumerable<CreateInventoryAdjustmentResponse>> GetAllAdjustmentsAsync()
    {
        return await context.InventoryAdjustments
            .Select(a => new CreateInventoryAdjustmentResponse(
                a.Warehouse.Name,
                a.Material.Name,
                a.NewQuantity,
                a.Reason,
                a.CreatedAt)
            )
            .ToListAsync();
    }
}
