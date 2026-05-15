using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.Productions.DTOs;

public record CreateProductionOrderRequest(
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
    int ProductId,
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer")]
    int QuantityToProduce,
    [Range(1, int.MaxValue, ErrorMessage = "Warehouse id must be a positive integer")]
    int WarehouseId);
