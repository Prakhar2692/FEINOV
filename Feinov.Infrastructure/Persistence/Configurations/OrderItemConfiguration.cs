using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasColumnName("order_item_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(item => item.OrderId)
            .HasColumnName("order_id");

        builder.Property(item => item.VariantId)
            .HasColumnName("variant_id");

        builder.Property(item => item.ProductName)
            .HasColumnName("product_name")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(item => item.VariantName)
            .HasColumnName("variant_name")
            .HasMaxLength(200);

        builder.Property(item => item.Sku)
            .HasColumnName("sku")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .HasColumnName("quantity");

        builder.Property(item => item.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("decimal(18,2)");

        builder.Property(item => item.DiscountAmount)
            .HasColumnName("discount_amount")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(item => item.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(18,2)");

        builder.HasOne(item => item.Order)
            .WithMany(order => order.Items)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_order_items_order");

        builder.HasOne(item => item.Variant)
            .WithMany(variant => variant.OrderItems)
            .HasForeignKey(item => item.VariantId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_order_items_variant");
    }
}
