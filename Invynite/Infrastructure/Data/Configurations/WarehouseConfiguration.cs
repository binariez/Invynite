using Invynite.Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// 1 to M with Inventory, with this being the principal entity.
///     Refer to InventoryConfiguration to configure the relationship between both entities.
///     
/// 1 to M with StockMovement, with this being the principal entity.
///     Refer to StockMovementConfiguration to configure the relationship between both entities.
/// </summary>
public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}
