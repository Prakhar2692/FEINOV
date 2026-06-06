using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("customer_addresses");

        builder.HasKey(address => address.Id);

        builder.Property(address => address.Id)
            .HasColumnName("address_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(address => address.UserId)
            .HasColumnName("user_id");

        builder.Property(address => address.AddressType)
            .HasColumnName("address_type")
            .HasMaxLength(50);

        builder.Property(address => address.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(address => address.MobileNumber)
            .HasColumnName("mobile_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(address => address.AddressLine1)
            .HasColumnName("address_line1")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(address => address.AddressLine2)
            .HasColumnName("address_line2")
            .HasMaxLength(500);

        builder.Property(address => address.Landmark)
            .HasColumnName("landmark")
            .HasMaxLength(200);

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
            .HasDefaultValue("India");

        builder.Property(address => address.IsDefault)
            .HasColumnName("is_default")
            .HasDefaultValue(false);

        builder.Property(address => address.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(address => address.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasOne(address => address.User)
            .WithMany(user => user.Addresses)
            .HasForeignKey(address => address.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_customer_addresses_user");
    }
}
