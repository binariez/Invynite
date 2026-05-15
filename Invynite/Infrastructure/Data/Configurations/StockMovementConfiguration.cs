using Invynite.Domain.Entities.Inven;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;


/// <summary>
/// Relationship to other entities:
/// 1 to 1 with Warehouse, with this being the dependant entity.
/// </summary>
public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Quantity)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.MovementType)
            .HasMaxLength(50)
            .IsRequired();

        // Warehouse relationship
        builder
            .HasOne(s => s.Warehouse)
            .WithMany()
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product relationship
        builder
            .HasOne(s => s.Product)
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Material relationship
        builder
            .HasOne(s => s.Material)
            .WithMany()
            .HasForeignKey(s => s.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        // prevent both ProductId and MaterialId from being null
        builder
            .ToTable(t =>
                t.HasCheckConstraint(
                    "CK_StockMovement_ProductOrMaterial",
                    """
                    ("ProductId" IS NOT NULL AND "MaterialId" IS NULL)
                    OR
                    ("ProductId" IS NULL AND "MaterialId" IS NOT NULL)
                    """
                    )
                );
    }
}
