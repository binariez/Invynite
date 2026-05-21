using Invynite.Domain.Entities.Production;
using Invynite.Domain.Entities.BOM;

namespace Invynite.Domain.Entities.Master;

public class Product
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public BillOfMaterial? BillOfMaterial { get; set; }
    public ICollection<ProductionOrder> ProductionOrders { get; set; } = [];
}
