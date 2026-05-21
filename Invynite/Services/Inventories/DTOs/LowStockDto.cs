namespace Invynite.Services.Inventories.DTOs
{
    public record LowStockDto(
        int MaterialId,
        string MaterialName,
        IEnumerable<LowStockDetail> Details
        );
}
