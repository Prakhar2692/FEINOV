using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("order_items")]
public partial class OrderItem
{
    [Key]
    [Column("order_item_id")]
    public Guid OrderItemId { get; set; }

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("variant_id")]
    public Guid VariantId { get; set; }

    [Column("product_name")]
    [StringLength(300)]
    public string ProductName { get; set; } = null!;

    [Column("variant_name")]
    [StringLength(200)]
    public string? VariantName { get; set; }

    [Column("sku")]
    [StringLength(100)]
    public string Sku { get; set; } = null!;

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("unit_price")]
    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    [Column("discount_amount")]
    [Precision(18, 2)]
    public decimal DiscountAmount { get; set; }

    [Column("total_amount")]
    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("OrderItems")]
    public virtual ProductVariant Variant { get; set; } = null!;
}
