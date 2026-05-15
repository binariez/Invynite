using Invynite.Domain.Entities.Purchasing;

namespace Invynite.Domain.Entities.Master;

public class Supplier
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = [];
}
