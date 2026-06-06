using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id)
            .HasColumnName("order_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(order => order.OrderNumber)
            .HasColumnName("order_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(order => order.CustomerId)
            .HasColumnName("customer_id");

        builder.Property(order => order.OrderStatus)
            .HasColumnName("order_status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(order => order.SubtotalAmount)
            .HasColumnName("subtotal_amount")
            .HasColumnType("decimal(18,2)");

        builder.Property(order => order.DiscountAmount)
            .HasColumnName("discount_amount")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(order => order.ShippingAmount)
            .HasColumnName("shipping_amount")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(order => order.TaxAmount)
            .HasColumnName("tax_amount")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(order => order.TotalAmount)
            .HasColumnName("total_amount")
            .HasColumnType("decimal(18,2)");

        builder.Property(order => order.PaymentStatus)
            .HasColumnName("payment_status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(order => order.Notes)
            .HasColumnName("notes");

        builder.Property(order => order.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(order => order.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasOne(order => order.Customer)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_orders_customer");

        builder.HasIndex(order => order.OrderNumber)
            .IsUnique();
    }
}
