using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("inventory");

        builder.HasKey(inventory => inventory.VariantId);

        builder.Property(inventory => inventory.VariantId)
            .HasColumnName("variant_id");

        builder.Property(inventory => inventory.TotalStock)
            .HasColumnName("total_stock")
            .HasDefaultValue(0);

        builder.Property(inventory => inventory.ReservedStock)
            .HasColumnName("reserved_stock")
            .HasDefaultValue(0);

        builder.Property(inventory => inventory.AvailableStock)
            .HasColumnName("available_stock")
            .HasDefaultValue(0);

        builder.Property(inventory => inventory.ReorderLevel)
            .HasColumnName("reorder_level")
            .HasDefaultValue(10);

        builder.Property(inventory => inventory.LastStockUpdated)
            .HasColumnName("last_stock_updated")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(inventory => inventory.Variant)
            .WithOne(variant => variant.Inventory)
            .HasForeignKey<Inventory>(inventory => inventory.VariantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_inventory_variant");
    }
}
