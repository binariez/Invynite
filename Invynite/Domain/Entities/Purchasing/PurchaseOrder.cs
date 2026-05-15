using Invynite.Domain.Entities.Master;
using Invynite.Domain.Enums;

namespace Invynite.Domain.Entities.Purchasing;

public class PurchaseOrder
{
    public int Id { get; init; }
    public int SupplierId {  get; set; }
    public Supplier Supplier { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = [];
}
