namespace Invynite.Services.Inventories.DTOs
{
    public record SimpleMaterialResponse(
        int Id,
        string Name,
        decimal CurrentStock,
        string UnitOfMeasure);
}
