using Invynite.Domain.Entities.Master;

namespace Invynite.Domain.Entities.Production;

/// <summary>
/// Intended for auditing purpose
/// </summary>
public class ProductionConsumption
{
    public int Id { get; init; }
    public int ProductionOrderId { get; set; }
    public ProductionOrder ProductionOrder { get; set; } = null!;
    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;
    public decimal QuantityUsed { get; set; }
}