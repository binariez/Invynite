using Invynite.Domain.Entities.BOM;
using Invynite.Domain.Entities.Inven;
using Invynite.Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Infrastructure.Data;

public static class Seeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        using var transaction = await context.Database.BeginTransactionAsync();
        try {

            // 1. Create Master Data
            var steel = new Material { Name = "Steel", UnitOfMeasure = "Kg" };
            var paint = new Material { Name = "Paint", UnitOfMeasure = "Liter" };
            var screws = new Material { Name = "Screws", UnitOfMeasure = "Pieces" };
            var chair = new Product { Name = "Chair", SKU = "CHR-001", UnitOfMeasure = "Pieces" };

            // 2. Create the BOM and LINK the Product object directly
            var bom = new BillOfMaterial
            {
                Product = chair
            };

            // 3. Add Items to the NAVIGATION COLLECTION instead of setting IDs
            // This forces EF to manage the dependency order correctly
            bom.BillOfMaterialItems = new List<BillOfMaterialItem>
            {
                new() { Material = steel, QuantityRequired = 5 },
                new() { Material = paint, QuantityRequired = 1 },
                new() { Material = screws, QuantityRequired = 20 }
            };

            // 4. Add the top-level objects
            // EF will walk down the 'bom' object and find the chair and the items
            await context.BillOfMaterials.AddAsync(bom);

            // Add other unrelated master data
            await context.Warehouses.AddAsync(new Warehouse { Name = "Main Warehouse", Location = "Factory Area" });

            await context.Suppliers.AddAsync(new Supplier
            {
                Name = "PT. Maju Jaya Selalu",
                ContactPerson = "Budi Santoso",
                Address = "Jl. Anggrek Nila No. 23C Medan, Sumatera Utara",
                PhoneNumber = "081376289836",
                Email = "sales@majujayamedan.com"
            });

            // 5. Final Save
            // EF Core 10 will now calculate the correct insertion order: 
            // Materials/Product -> BOM -> BOMItems
            await context.SaveChangesAsync();

            var inventories = new List<Inventory>
            {
                new () { MaterialId = 1, Quantity = 0, WarehouseId = 1 },
                new () { MaterialId = 2, Quantity = 0, WarehouseId = 1 },
                new () { MaterialId = 3, Quantity = 0, WarehouseId = 1 }
            };

            await context.Inventories.AddRangeAsync(inventories);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}