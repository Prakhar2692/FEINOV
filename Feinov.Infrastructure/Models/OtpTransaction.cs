using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("otp_transactions")]
public partial class OtpTransaction
{
    [Key]
    [Column("otp_transaction_id")]
    public Guid OtpTransactionId { get; set; }

    [Column("mobile_number")]
    [StringLength(20)]
    public string MobileNumber { get; set; } = null!;

    [Column("otp_code")]
    [StringLength(10)]
    public string OtpCode { get; set; } = null!;

    [Column("purpose")]
    [StringLength(50)]
    public string Purpose { get; set; } = null!;

    [Column("is_used")]
    public bool IsUsed { get; set; }

    [Column("expires_at", TypeName = "timestamp without time zone")]
    public DateTime ExpiresAt { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }
}
