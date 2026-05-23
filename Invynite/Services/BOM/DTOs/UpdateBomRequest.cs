namespace Invynite.Services.BOM.DTOs
{
    public record UpdateBomRequest(Dictionary<int, decimal> Recipes);
}
