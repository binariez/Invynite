namespace Invynite.Services.Inventories.DTOs
{
    public record MaterialResponse(
        int Id,
        string Name,
        decimal CurrentStock,
        string UnitOfMeasure,
        IEnumerable<string> StoredAtWarehouses);
}
