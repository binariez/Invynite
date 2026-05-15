using Invynite.Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to 1 with BillOfMaterial, with this being the principal entity.
///     Refer to BillOfMaterialConfiguration to configure the relationship between both entities.
///     
/// 1 to 1 with ProductionOrder, with this being the principal entity.
///     Refer to ProductionOrderConfiguration to configure the relationship between both entities.
///     
/// 1 to 1 with Inventory, with this being the principal entity.
///     Refer to InventoryConfiguration to configure the relationship between both entities.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.SKU)
            .IsUnique();

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}
