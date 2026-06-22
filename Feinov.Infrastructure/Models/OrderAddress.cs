using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("order_addresses")]
[Index("OrderId", Name = "order_addresses_order_id_key", IsUnique = true)]
public partial class OrderAddress
{
    [Key]
    [Column("order_address_id")]
    public Guid OrderAddressId { get; set; }

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("full_name")]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Column("address_line1")]
    [StringLength(500)]
    public string AddressLine1 { get; set; } = null!;

    [Column("address_line2")]
    [StringLength(500)]
    public string? AddressLine2 { get; set; }

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

    [ForeignKey("OrderId")]
    [InverseProperty("OrderAddress")]
    public virtual Order Order { get; set; } = null!;
}
