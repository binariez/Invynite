using Invynite.Domain.Entities.Inven;
using Invynite.Domain.Entities.Production;
using Invynite.Domain.Enums;
using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.Productions.DTOs;
using Invynite.Services.Productions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Linq.Expressions;

namespace Invynite.Services.Productions;

public class ProductionOrderService(AppDbContext context) : IProductionOrderService
{
    private readonly AppDbContext context = context;

    public async Task<CreateProductionOrderResponse> Produce(CreateProductionOrderRequest request)
    {
        // calculate required materials for the product
        var recipe = await context.BillOfMaterialItems
            .Where(i => i.BillOfMaterial.ProductId == request.ProductId)
            .Select(i => new
            {
                i.MaterialId,
                i.Material.Name,
                QuantityRequired = i.QuantityRequired * request.QuantityToProduce
            })
            .ToListAsync();

        if (recipe.Count == 0)
            throw new NotFoundException($"BOM for product with id: {request.ProductId} can't be found.");

        var materialIds = recipe.Select(r => r.MaterialId).ToList();

        // store inventory data inside dictionary for fast lookup in foreach loop
        var inventories = await context.Inventories
            .Where(i =>
                i.WarehouseId == request.WarehouseId &&
                i.MaterialId != null &&
                materialIds.Contains(i.MaterialId.Value)
            )
            .ToDictionaryAsync(i => i.MaterialId!.Value);

        if (inventories.Count == 0)
            throw new NotFoundException("No material found");

        // begin processing the db update with transaction
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // add new production order
            var productionOrder = new ProductionOrder
            {
                ProductId = request.ProductId,
                QuantityToProduce = request.QuantityToProduce,
                WarehouseId = request.WarehouseId,
                Status = ProductionStatus.Completed
            };

            await context.ProductionOrders.AddAsync(productionOrder);

            await context.SaveChangesAsync();

            var stockMovements = new List<StockMovement>();

            foreach (var material in recipe)
            {
                // take entity from db to be updated
                if(!inventories.TryGetValue(material.MaterialId, out var inventory) ||
                    inventory.Quantity < material.QuantityRequired)
                {
                    throw new InsufficientStockException($"Operation breaks because insufficient stock of material: {material.Name}");
                }

                // deduct materials from the entity
                inventory.Quantity -= material.QuantityRequired;

                // create logs for materials
                stockMovements.Add(new StockMovement
                {
                    MaterialId = material.MaterialId,
                    Quantity = material.QuantityRequired,
                    WarehouseId = request.WarehouseId,
                    MovementType = MovementType.OUT,
                    ReferenceId = productionOrder.Id
                });
            }

            // adding the newly made product to inventory
            // or updating the amount if it already exists in inventory
            var productInventory = await context.Inventories.FirstOrDefaultAsync(i =>
                i.ProductId == request.ProductId &&
                i.WarehouseId == request.WarehouseId);

            if (productInventory is null)
            {
                productInventory = new Inventory
                {
                    ProductId = request.ProductId,
                    Quantity = request.QuantityToProduce,
                    WarehouseId = request.WarehouseId
                };

                await context.Inventories.AddAsync(productInventory);
            }
            else
            {
                productInventory.Quantity += request.QuantityToProduce;
            }

            // create log for new product
            stockMovements.Add(new StockMovement
            {
                ProductId = request.ProductId,
                Quantity = request.QuantityToProduce,
                WarehouseId = request.WarehouseId,
                MovementType = MovementType.IN,
                ReferenceId = productionOrder.Id
            });

            // add all collected stock movements
            await context.StockMovements.AddRangeAsync(stockMovements);

            await context.SaveChangesAsync();

            // commit trasaction
            await transaction.CommitAsync();

            return new CreateProductionOrderResponse(productionOrder.Id, productionOrder.Status.GetDisplayName());
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<GetProductionOrderResponse>> GetProductionOrderHistory()
    {
        var result = await context.ProductionOrders
            .OrderByDescending(po => po.CreatedAt)
            .Select(Projection)
            .ToListAsync();

        return result;
    }

    public async Task<GetProductionOrderResponse?> GetProductionOrderById(int orderId)
    {
        var result = await context.ProductionOrders
            .Where(po => po.Id == orderId)
            .Select(Projection)
            .FirstOrDefaultAsync();

        return result ?? throw new NotFoundException($"Product order with id: {orderId} can't be found");
    }

    private static Expression<Func<ProductionOrder, GetProductionOrderResponse>> Projection =>
        po => new GetProductionOrderResponse(
            po.Id,
            po.ProductId,
            po.Product.Name,
            po.QuantityToProduce,
            po.Warehouse.Name,
            po.Status.GetDisplayName(),
            po.CreatedAt
        );
}
