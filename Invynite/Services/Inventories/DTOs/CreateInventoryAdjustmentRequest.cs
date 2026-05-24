using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.Inventories.DTOs
{
    public record CreateInventoryAdjustmentRequest(
        [Range(1, int.MaxValue,ErrorMessage = "Warehouse id must be a positive integer")]
        int WarehouseId,
        [Range(1, int.MaxValue,ErrorMessage = "Material id must be a positive integer")]
        int MaterialId,
        [Range(1, 99999.99)]
        decimal NewQuantity,
        [Required(ErrorMessage = "Reason required")]
        string Reason);
}
