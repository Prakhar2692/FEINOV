using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("user_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(user => user.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100);

        builder.Property(user => user.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100);

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(255);

        builder.Property(user => user.MobileNumber)
            .HasColumnName("mobile_number")
            .HasMaxLength(20);

        builder.Property(user => user.IsEmailVerified)
            .HasColumnName("is_email_verified")
            .HasDefaultValue(false);

        builder.Property(user => user.IsMobileVerified)
            .HasColumnName("is_mobile_verified")
            .HasDefaultValue(false);

        builder.Property(user => user.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(user => user.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(user => user.UpdatedDate)
            .HasColumnName("updated_date");
    }
}
