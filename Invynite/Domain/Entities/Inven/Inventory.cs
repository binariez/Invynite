using Invynite.Domain.Entities.Master;

namespace Invynite.Domain.Entities.Inven;

public class Inventory
{
    public int Id { get; init; }
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    /// Either ProductId or MaterialId is filled, not both
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
    public int? MaterialId { get; set; }
    public Material? Material { get; set; }
    public decimal Quantity { get; set; }
    public byte[] RowVersion { get; set; } = default!;
}