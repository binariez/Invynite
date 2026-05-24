using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.BOM.DTOs
{
    public record CreateBomRequest(
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
        int ProductId,
        Dictionary<int, decimal> Recipes);
}