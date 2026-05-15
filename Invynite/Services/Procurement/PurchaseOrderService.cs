using Invynite.Domain.Entities.Inven;
using Invynite.Domain.Entities.Purchasing;
using Invynite.Domain.Enums;
using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.Procurement.DTOs;
using Invynite.Services.Procurement.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace Invynite.Services.Procurement
{
    public class PurchaseOrderService(AppDbContext context) : IPurchaseOrderService
    {
        public async Task<CreatePurchaseOrderResponse> CreatePurchaseOrder(CreatePurchaseOrderRequest purchaseOrderRequest)
        {
            // check for material id duplicates
            var duplicateIds = purchaseOrderRequest.PurchaseOrderItems
                .GroupBy(i => i.MaterialId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateIds.Count > 0)
                throw new BadRequestException("Duplicate materials not allowed");

            // check if supplier exists
            if (await context.Suppliers.AnyAsync(s => s.Id == purchaseOrderRequest.SupplierId) == false)
                throw new NotFoundException($"Supplier with id: {purchaseOrderRequest.SupplierId} can't be found");

            // take material ids from request
            var requestedMaterialIds = purchaseOrderRequest.PurchaseOrderItems
                    .Select(poi => poi.MaterialId)
                    .Distinct()
                    .ToList();

            // compare requested materials with their existence in the database
            var existingMaterialIds = await context.Materials
                .Where(m => requestedMaterialIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            // check if one of more requested material doesn't have data in database
            // by comparing requested materials with existing materials
            var invalidIds = requestedMaterialIds
                .Except(existingMaterialIds)
                .ToList();

            if (invalidIds.Count != 0)
                throw new NotFoundException("One of more material doesn't exist in database");

            var purchaseOrder = new PurchaseOrder
            {
                SupplierId = purchaseOrderRequest.SupplierId,
                Status = PurchaseOrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                PurchaseOrderItems = purchaseOrderRequest.PurchaseOrderItems
                    .Select(poi => new PurchaseOrderItem
                    {
                        MaterialId = poi.MaterialId,
                        Quantity = poi.Quantity,
                        UnitPrice = poi.UnitPrice
                    })
                    .ToList()
            };

            await context.PurchaseOrders.AddAsync(purchaseOrder);

            await context.SaveChangesAsync();

            return new CreatePurchaseOrderResponse(purchaseOrder.Id, purchaseOrder.Status.GetDisplayName());
        }

        /// <summary>
        /// Set the purchase status to `Received`.
        /// Updating the inventory stock based on the purchased material list
        /// </summary>
        /// <param name="purchaseOrderId">The purchase order ID which will be updated</param>
        /// <param name="warehouseId">The warehouse which the items will be stored at</param>
        public async Task<ReceivePurchaseOrderResponse> ReceivePurchaseOrder(int purchaseOrderId, int warehouseId)
        {
            if (await context.Warehouses.AnyAsync(w => w.Id == warehouseId) == false)
                throw new NotFoundException($"Warehouse with id: {warehouseId} does not exist");

            var purchaseOrder = await context.PurchaseOrders
                .Include(po => po.PurchaseOrderItems)
                    .ThenInclude(poi => poi.Material)
                .FirstOrDefaultAsync(po => po.Id == purchaseOrderId);

            if (purchaseOrder == null)
                throw new NotFoundException($"Purchase order with id: {purchaseOrderId} does not exist.");

            if (purchaseOrder.Status != PurchaseOrderStatus.Pending)
                throw new PurchaseOrderNotPendingException("Can't receive the order because the status is not pending");

            var purchasedItems = purchaseOrder.PurchaseOrderItems
                .Select(i => new ItemReceivedDto(i.MaterialId, i.Material.Name, i.Quantity))
                .ToList();

            var inventories = await context.Inventories
                .Where(inv => 
                    inv.WarehouseId == warehouseId &&
                    inv.MaterialId != null)
                .ToDictionaryAsync(inv => inv.MaterialId!.Value);

            var stockMovements = new List<StockMovement>();

            var newMaterials = new List<Inventory>();

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in purchasedItems)
                {
                    if (inventories.TryGetValue(item.MaterialId, out var inventory))
                    {
                        inventory.Quantity += item.Quantity;
                    }
                    else
                    {
                        newMaterials.Add(new Inventory
                        {
                            MaterialId = item.MaterialId,
                            Quantity = item.Quantity,
                            WarehouseId = warehouseId
                        });
                    }

                    stockMovements.Add(new StockMovement
                    {
                        MaterialId = item.MaterialId,
                        Quantity = item.Quantity,
                        WarehouseId = warehouseId,
                        MovementType = "IN",
                        CreatedAt = DateTime.UtcNow,
                        ReferenceId = purchaseOrderId
                    });
                }

                await context.Inventories.AddRangeAsync(newMaterials);

                await context.StockMovements.AddRangeAsync(stockMovements);

                purchaseOrder.Status = PurchaseOrderStatus.Received;

                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ReceivePurchaseOrderResponse(purchaseOrderId, purchasedItems);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<GetPurchaseOrderResponse>> GetPurchaseOrderHistory()
        {
            var result = await context.PurchaseOrders
                .Select(po => new GetPurchaseOrderResponse(
                    po.Id,
                    po.Supplier.Name,
                    po.OrderDate,
                    po.Status.GetDisplayName()
                    )
                )
                .ToListAsync();

            return result;
        }
    }
}
