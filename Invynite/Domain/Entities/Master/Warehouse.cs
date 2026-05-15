using Invynite.Domain.Entities.Inven;
using Invynite.Domain.Entities.Production;

namespace Invynite.Domain.Entities.Master;

public class Warehouse
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ICollection<Inventory> Inventories { get; set; } = [];
    public ICollection<ProductionOrder> ProductionOrders { get; set; } = [];
}
