using Invynite.Domain.Entities.Master;

namespace Invynite.Domain.Entities.BOM;

public class BillOfMaterial
{
    public int Id { get; init; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public ICollection<BillOfMaterialItem> BillOfMaterialItems { get; set; } = [];
}
