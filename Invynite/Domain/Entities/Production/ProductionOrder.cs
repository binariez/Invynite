using Invynite.Domain.Entities.Master;
using Invynite.Domain.Enums;

namespace Invynite.Domain.Entities.Production;

public class ProductionOrder
{
    public int Id { get; init; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int QuantityToProduce { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public ProductionStatus Status { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public ICollection<ProductionConsumption> ProductionConsumptions { get; set; } = [];
    public byte[] RowVersion { get; set; } = default!;
}