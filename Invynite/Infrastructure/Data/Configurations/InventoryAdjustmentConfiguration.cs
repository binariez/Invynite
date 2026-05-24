using Invynite.Domain.Entities.Inven;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to Many with Warehouse, with this being the dependant entity.
/// 1 to Many with Product, with this being the dependant entity.
/// 1 to Many with Material, with this being the dependant entity.
/// </summary>
public class InventoryAdjustmentConfiguration : IEntityTypeConfiguration<InventoryAdjustment>
{
    public void Configure(EntityTypeBuilder<InventoryAdjustment> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Reason)
            .HasMaxLength(150);

        builder
            .Property(x => x.PreviousQuantity)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.NewQuantity)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.Difference)
            .HasPrecision(18, 2);

        // Warehouse relationship
        builder
            .HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product relationship
        //builder
        //    .HasOne(x => x.Product)
        //    .WithMany()
        //    .HasForeignKey(x => x.ProductId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // Material relationship
        builder
            .HasOne(x => x.Material)
            .WithMany()
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent both id from being null
        //builder
        //    .ToTable(t =>
        //    t.HasCheckConstraint(
        //        "CK_Adjustment_ProductOrMaterial",
        //        """
        //        ("ProductId" IS NOT NULL AND "MaterialId" IS NULL)
        //        OR
        //        ("MaterialId" IS NOT NULL AND "ProductId" IS NULL)
        //        """
        //        )
        //    );
    }
}
