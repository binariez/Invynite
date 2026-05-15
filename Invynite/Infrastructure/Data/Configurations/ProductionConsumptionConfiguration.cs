using Invynite.Domain.Entities.Production;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invynite.Infrastructure.Data.Configurations;

/// <summary>
/// Relationship to other entities:
/// 1 to M with ProductionOrder, with this being the dependant entity.
/// 
/// 1 to M with Material, with this being the dependant entity.
/// </summary>
public class ProductionConsumptionConfiguration : IEntityTypeConfiguration<ProductionConsumption>
{
    public void Configure(EntityTypeBuilder<ProductionConsumption> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.QuantityUsed)
            .HasPrecision(18, 2);

        // prevnt duplicate material for the same production order
        builder.HasIndex(x => new
        {
            x.ProductionOrderId,
            x.MaterialId
        }).IsUnique();

        // ProductionOrder relationship
        builder.HasOne(c => c.ProductionOrder)
            .WithMany(o => o.ProductionConsumptions)
            .HasForeignKey(c => c.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Material relationship
        builder.HasOne(c => c.Material)
            .WithMany(m => m.ProductionConsumptions)
            .HasForeignKey(c => c.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
