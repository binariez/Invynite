using Invynite.Domain.Enums;

namespace Invynite.Services.Productions.DTOs
{
    public record GetStockMovementResponse(
        int Id,
        string ItemType,
        string ItemName,
        decimal Quantity,
        MovementType MovementType,
        string WarehouseName,
        int ReferenceId,
        DateTime CreatedAt
        );
}
