using Invynite.Domain.Entities.Master;

namespace Invynite.Domain.Entities.Inven
{
    public class InventoryAdjustment
    {
        public int Id { get; init; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = null!;
        //public int? ProductId { get; set; }
        //public Product? Product { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        public decimal PreviousQuantity { get; set; }
        public decimal NewQuantity { get; set; }
        public decimal Difference { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
