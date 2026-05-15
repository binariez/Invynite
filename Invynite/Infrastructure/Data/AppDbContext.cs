using Invynite.Domain.Entities.BOM;
using Invynite.Domain.Entities.Inven;
using Invynite.Domain.Entities.Master;
using Invynite.Domain.Entities.Production;
using Invynite.Domain.Entities.Purchasing;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<BillOfMaterial> BillOfMaterials => Set<BillOfMaterial>();
        public DbSet<BillOfMaterialItem> BillOfMaterialItems => Set<BillOfMaterialItem>();
        public DbSet<ProductionOrder> ProductionOrders => Set<ProductionOrder>();
        public DbSet<ProductionConsumption> ProductionConsumptions => Set<ProductionConsumption>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Enum>().HaveConversion<string>();
        }
    }
}
