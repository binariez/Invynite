using Invynite.Domain.Entities.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to M with PurchaseOrder, with this being the dependant entity.
/// 1 to M with Material, with this being the dependant entity.s
/// </summary>
public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Quantity)
            .HasPrecision(18, 2);

        builder
            .Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        // PurchaseOrder relationship
        builder
            .HasOne(i => i.PurchaseOrder)
            .WithMany(p => p.PurchaseOrderItems)
            .HasForeignKey(i => i.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Material relationship
        builder
            .HasOne(i => i.Material)
            .WithMany(m => m.PurchaseOrderItems)
            .HasForeignKey(i => i.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
