using Invynite.Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to M with PurchaseOrder, with this being the principal entity.
///     Refer to PurchaseOrderConfiguration to configure the relationship between both entities.
/// </summary>
public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.PhoneNumber)
            .IsUnique();
    }
}
