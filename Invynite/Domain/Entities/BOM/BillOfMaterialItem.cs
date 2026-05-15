using Invynite.Domain.Entities.Master;

namespace Invynite.Domain.Entities.BOM;

public class BillOfMaterialItem
{
    public int Id { get; init; }
    public int BillOfMaterialId { get; set; }
    public BillOfMaterial BillOfMaterial { get; set; } = new();
    public int MaterialId { get; set; }
    public Material Material { get; set; } = new();
    public decimal QuantityRequired { get; set; }
}
