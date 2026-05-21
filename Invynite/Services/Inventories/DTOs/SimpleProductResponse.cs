namespace Invynite.Services.Inventories.DTOs
{
    public record SimpleProductResponse(
        int Id,
        string Name,
        string SKU,
        decimal CurrentStock,
        string UnitOfMeasure);
}
