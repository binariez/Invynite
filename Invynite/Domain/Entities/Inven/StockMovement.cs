using Invynite.Domain.Entities.Master;
using Invynite.Domain.Enums;

namespace Invynite.Domain.Entities.Inven;

/// <summary>
/// Intended for auditing purpose
/// </summary>
public class StockMovement
{
    public int Id { get; init; }
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public int? MaterialId { get; set; }
    public Material? Material { get; set; }
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public int ReferenceId { get; set; } // ProductionOrderId, PurchaseId, etc.
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}