using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.Procurement.DTOs
{
    public record CreatePurchaseOrderRequest(
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Supplier id must be a positive integer")]
        int SupplierId,
        List<PurchaseOrderItemDto> PurchaseOrderItems
        );
}
