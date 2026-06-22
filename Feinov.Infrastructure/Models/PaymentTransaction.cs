using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("payment_transactions")]
public partial class PaymentTransaction
{
    [Key]
    [Column("payment_transaction_id")]
    public Guid PaymentTransactionId { get; set; }

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("payment_provider")]
    [StringLength(50)]
    public string PaymentProvider { get; set; } = null!;

    [Column("provider_order_id")]
    [StringLength(200)]
    public string? ProviderOrderId { get; set; }

    [Column("provider_payment_id")]
    [StringLength(200)]
    public string? ProviderPaymentId { get; set; }

    [Column("provider_signature")]
    [StringLength(500)]
    public string? ProviderSignature { get; set; }

    [Column("transaction_amount")]
    [Precision(18, 2)]
    public decimal TransactionAmount { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [Column("transaction_status")]
    [StringLength(50)]
    public string TransactionStatus { get; set; } = null!;

    [Column("gateway_response")]
    public string? GatewayResponse { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("PaymentTransactions")]
    public virtual Order Order { get; set; } = null!;
}
