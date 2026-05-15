using Invynite.Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to M with BillOfMaterialItem, with this being the principal entity.
///     Refer to BillOfMaterialItemConfiguration to configure the relationship between both entities.
///     
/// 1 to M with ProductionConfumption, with this being the principal entity.
///     Refer to ProductionConfumptionConfiguration to configure the relationship between both entities.
///     
/// 1 to 1 with Inventory, with this being the principal entity.
///     Refer to InventoryConfiguration to configure the relationship between both entities.
///     
/// 1 to 1 with PurchaseOrderItem, with this being the principal entity.
///     Refer to PurchaseOrderItemConfiguration to configure the relationship between both entities.
/// </summary>
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}
