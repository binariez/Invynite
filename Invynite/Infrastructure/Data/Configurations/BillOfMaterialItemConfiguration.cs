using Invynite.Domain.Entities.BOM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to M with BillOfMaterial, with this being the dependant entity.
/// 1 to M with Material, with this being the dependant entity.
/// </summary>
public class BillOfMaterialItemConfiguration : IEntityTypeConfiguration<BillOfMaterialItem>
{
    public void Configure(EntityTypeBuilder<BillOfMaterialItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.QuantityRequired)
            .HasPrecision(18, 2);

        // prevent duplicate material inside for the same bill of material
        builder.HasIndex(i => new
        {
            i.BillOfMaterialId,
            i.MaterialId
        }).IsUnique();

        // BillOfMaterial relationsip
        builder
            .HasOne(i => i.BillOfMaterial)
            .WithMany(b => b.BillOfMaterialItems)
            .HasForeignKey(i => i.BillOfMaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        // Material relationship
        builder
            .HasOne(i => i.Material)
            .WithMany(m => m.BillOfMaterialItems)
            .HasForeignKey(i => i.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
