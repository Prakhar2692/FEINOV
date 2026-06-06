using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
{
    public void Configure(EntityTypeBuilder<OrderAddress> builder)
    {
        builder.ToTable("order_addresses");

        builder.HasKey(address => address.Id);

        builder.Property(address => address.Id)
            .HasColumnName("order_address_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(address => address.OrderId)
            .HasColumnName("order_id");

        builder.Property(address => address.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(address => address.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(address => address.AddressLine1)
            .HasColumnName("address_line1")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(address => address.AddressLine2)
            .HasColumnName("address_line2")
            .HasMaxLength(500);

        builder.Property(address => address.City)
            .HasColumnName("city")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(address => address.State)
            .HasColumnName("state")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(address => address.PostalCode)
            .HasColumnName("postal_code")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(address => address.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(address => address.Order)
            .WithOne(order => order.ShippingAddress)
            .HasForeignKey<OrderAddress>(address => address.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_order_address_order");

        builder.HasIndex(address => address.OrderId)
            .IsUnique();
    }
}
