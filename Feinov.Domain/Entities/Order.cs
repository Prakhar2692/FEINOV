using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }

    public string OrderStatus { get; set; } = string.Empty;

    public decimal SubtotalAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal ShippingAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public User Customer { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = [];

    public OrderAddress? ShippingAddress { get; set; }

    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = [];
}
