namespace Invynite.Services.BOM.DTOs
{
    public record CreateBomRequest(int ProductId, Dictionary<int, decimal> Recipes);
}