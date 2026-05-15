using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.Procurement.DTOs
{
    public record PurchaseOrderItemDto(
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Material id must be a positive integer")]
        int MaterialId,
        [Required]
        [Range(1, 99999.99)]
        decimal Quantity,
        [Range(1, 99999999.99)]
        decimal UnitPrice);
}
