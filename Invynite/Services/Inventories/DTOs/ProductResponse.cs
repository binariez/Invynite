namespace Invynite.Services.Inventories.DTOs
{
    public record ProductResponse(
        int Id,
        string Name,
        string SKU,
        decimal CurrentStock,
        string UnitOfMeasure,
        Dictionary<string, decimal> StoredAtWarehouses);
}
