using Invynite.Domain.Enums;

namespace Invynite.Services.Productions.DTOs
{
    public record GetProductionOrderResponse(
        int OrderId,
        int ProductId,
        string ProductName,
        int QuantityToProduce,
        string WarehouseName,
        string Status,
        DateTime CreatedAt
        );
}
