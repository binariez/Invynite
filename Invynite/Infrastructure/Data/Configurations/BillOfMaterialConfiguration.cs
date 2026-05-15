using Invynite.Domain.Entities.BOM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to 1 with Product, with this being the dependant entity.
///     
/// 1 to M with BillOfMaterialItem, with this being the principal entity.
///     Refer to BillOfMaterialItemConfiguration to configure the relationship between both entities.
/// </summary>
public class BillOfMaterialConfiguration : IEntityTypeConfiguration<BillOfMaterial>
{
    public void Configure(EntityTypeBuilder<BillOfMaterial> builder)
    {
        builder.HasKey(x => x.Id);

        // prevent duplicate bill of material for the same product
        builder
            .HasIndex(x => x.ProductId)
            .IsUnique();

        // Product relationship
        builder
            .HasOne(b => b.Product)
            .WithOne(p => p.BillOfMaterial)
            .HasForeignKey<BillOfMaterial>(b => b.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
