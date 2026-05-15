using Invynite.Domain.Entities.Inven;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to 1 with Warehouse, with this being the dependant entity.
/// 1 to 1 with Product, with this being the dependant entity.
/// 1 to 1 with Material, with this being the dependant entity.
/// </summary>
public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Quantity)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();

        // Warehouse relationship
        builder
            .HasOne(i => i.Warehouse)
            .WithMany(w => w.Inventories)
            .HasForeignKey(i => i.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product relationship
        builder
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Material relationship
        builder
            .HasOne(i => i.Material)
            .WithMany()
            .HasForeignKey(i => i.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        // prevent both ProductId and MaterialId from being null
        builder
            .ToTable(t =>
                t.HasCheckConstraint(
                    "CK_Inventory_ProductOrMaterial",
                    """
                    ("ProductId" IS NOT NULL AND "MaterialId" IS NULL)
                    OR
                    ("ProductId" IS NULL AND "MaterialId" IS NOT NULL)
                    """
                    )
                );

        // prevent duplicate product at the same inventory in a warehouse
        builder
            .HasIndex(x => new
            {
                x.ProductId,
                x.WarehouseId
            })
            .IsUnique()
            .HasFilter("[ProductId] IS NOT NULL");

        // prevent duplicate material at the same inventory in a warehouse
        builder
            .HasIndex(x => new
            {
                x.MaterialId,
                x.WarehouseId
            })
            .IsUnique()
            .HasFilter("[MaterialId] IS NOT NULL");
    }
}
