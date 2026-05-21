using Invynite.Domain.Entities.BOM;
using Invynite.Domain.Entities.Production;
using Invynite.Domain.Entities.Purchasing;

namespace Invynite.Domain.Entities.Master;

public class Material
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public ICollection<BillOfMaterialItem> BillOfMaterialItems { get; set; } = [];
    public ICollection<ProductionConsumption> ProductionConsumptions { get; set; } = [];
    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = [];
}
