using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }

    public Guid VariantId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string? VariantName { get; set; }

    public string Sku { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public Order Order { get; set; } = null!;

    public ProductVariant Variant { get; set; } = null!;
}
