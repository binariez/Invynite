using Invynite.Domain.Entities.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to 1 with Product, with this being the dependant entity.
/// 
/// 1 to M with ProductionConsumption, with this being the principal entity.
///     Refer to ProductionConsumptionConfiguration to configure the relationship between both entities.
/// </summary>
public class ProductionOrderConfiguration : IEntityTypeConfiguration<ProductionOrder>
{
    public void Configure(EntityTypeBuilder<ProductionOrder> builder)
    {
        builder.HasKey(x => x.Id);

        builder
        .Property(x => x.Status)
        .HasMaxLength(50)
        .IsRequired();

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();

        // Product relationship
        builder
            .HasOne(o => o.Product)
            .WithMany(p => p.ProductionOrders)
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Warehouse relationship
        builder
            .HasOne(o => o.Warehouse)
            .WithMany(w => w.ProductionOrders)
            .HasForeignKey(o => o.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
