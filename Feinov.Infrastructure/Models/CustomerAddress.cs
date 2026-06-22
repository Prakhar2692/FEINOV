using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("customer_addresses")]
public partial class CustomerAddress
{
    [Key]
    [Column("address_id")]
    public Guid AddressId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("address_type")]
    [StringLength(50)]
    public string? AddressType { get; set; }

    [Column("full_name")]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [Column("mobile_number")]
    [StringLength(20)]
    public string MobileNumber { get; set; } = null!;

    [Column("address_line1")]
    [StringLength(500)]
    public string AddressLine1 { get; set; } = null!;

    [Column("address_line2")]
    [StringLength(500)]
    public string? AddressLine2 { get; set; }

    [Column("landmark")]
    [StringLength(200)]
    public string? Landmark { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Column("state")]
    [StringLength(100)]
    public string State { get; set; } = null!;

    [Column("postal_code")]
    [StringLength(20)]
    public string PostalCode { get; set; } = null!;

    [Column("country")]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    [Column("is_default")]
    public bool IsDefault { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CustomerAddresses")]
    public virtual User User { get; set; } = null!;
}
