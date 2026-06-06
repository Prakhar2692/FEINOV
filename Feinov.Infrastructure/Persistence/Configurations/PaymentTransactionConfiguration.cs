using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("payment_transactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Id)
            .HasColumnName("payment_transaction_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(transaction => transaction.OrderId)
            .HasColumnName("order_id");

        builder.Property(transaction => transaction.PaymentProvider)
            .HasColumnName("payment_provider")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(transaction => transaction.ProviderOrderId)
            .HasColumnName("provider_order_id")
            .HasMaxLength(200);

        builder.Property(transaction => transaction.ProviderPaymentId)
            .HasColumnName("provider_payment_id")
            .HasMaxLength(200);

        builder.Property(transaction => transaction.ProviderSignature)
            .HasColumnName("provider_signature")
            .HasMaxLength(500);

        builder.Property(transaction => transaction.TransactionAmount)
            .HasColumnName("transaction_amount")
            .HasColumnType("decimal(18,2)");

        builder.Property(transaction => transaction.PaymentMethod)
            .HasColumnName("payment_method")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(transaction => transaction.TransactionStatus)
            .HasColumnName("transaction_status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(transaction => transaction.GatewayResponse)
            .HasColumnName("gateway_response");

        builder.Property(transaction => transaction.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(transaction => transaction.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasOne(transaction => transaction.Order)
            .WithMany(order => order.PaymentTransactions)
            .HasForeignKey(transaction => transaction.OrderId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_payment_transaction_order");
    }
}
