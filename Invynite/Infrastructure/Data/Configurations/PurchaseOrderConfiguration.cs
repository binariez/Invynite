using Invynite.Domain.Entities.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to 1 with Supplier, with this being the dependant entity.
/// </summary>
public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();

        // Supplier relationship
        builder
            .HasOne(o => o.Supplier)
            .WithMany(s => s.PurchaseOrders)
            .HasForeignKey(o => o.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
