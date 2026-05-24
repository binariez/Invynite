namespace Invynite.Services.Inventories.DTOs
{
    public record CreateInventoryAdjustmentResponse(string WarehouseName, string MaterialName, decimal NewQuantity, string Reason, DateTime CreatedAt);
}
