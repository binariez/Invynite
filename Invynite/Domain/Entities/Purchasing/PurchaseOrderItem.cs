using Invynite.Domain.Entities.Master;

namespace Invynite.Domain.Entities.Purchasing;

public class PurchaseOrderItem
{
    public int Id { get; init; }
    public int PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}