using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class OtpTransactionConfiguration : IEntityTypeConfiguration<OtpTransaction>
{
    public void Configure(EntityTypeBuilder<OtpTransaction> builder)
    {
        builder.ToTable("otp_transactions");

        builder.HasKey(otp => otp.Id);

        builder.Property(otp => otp.Id)
            .HasColumnName("otp_transaction_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(otp => otp.MobileNumber)
            .HasColumnName("mobile_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(otp => otp.OtpCode)
            .HasColumnName("otp_code")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(otp => otp.Purpose)
            .HasColumnName("purpose")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(otp => otp.IsUsed)
            .HasColumnName("is_used")
            .HasDefaultValue(false);

        builder.Property(otp => otp.ExpiresAt)
            .HasColumnName("expires_at");

        builder.Property(otp => otp.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
