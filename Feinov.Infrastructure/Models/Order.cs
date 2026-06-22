using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("orders")]
[Index("OrderNumber", Name = "orders_order_number_key", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("order_number")]
    [StringLength(50)]
    public string OrderNumber { get; set; } = null!;

    [Column("customer_id")]
    public Guid CustomerId { get; set; }

    [Column("order_status")]
    [StringLength(50)]
    public string OrderStatus { get; set; } = null!;

    [Column("subtotal_amount")]
    [Precision(18, 2)]
    public decimal SubtotalAmount { get; set; }

    [Column("discount_amount")]
    [Precision(18, 2)]
    public decimal DiscountAmount { get; set; }

    [Column("shipping_amount")]
    [Precision(18, 2)]
    public decimal ShippingAmount { get; set; }

    [Column("tax_amount")]
    [Precision(18, 2)]
    public decimal TaxAmount { get; set; }

    [Column("total_amount")]
    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    [Column("payment_status")]
    [StringLength(50)]
    public string PaymentStatus { get; set; } = null!;

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual User Customer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual OrderAddress? OrderAddress { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}
