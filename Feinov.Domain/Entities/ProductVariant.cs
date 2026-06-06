using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public Guid ProductId { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string VariantName { get; set; } = string.Empty;

    public int? SizeMl { get; set; }

    public int PackSize { get; set; } = 1;

    public decimal? WeightGrams { get; set; }

    public decimal Mrp { get; set; }

    public string? Barcode { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Product Product { get; set; } = null!;

    public Inventory? Inventory { get; set; }

    public ICollection<VariantDiscount> Discounts { get; set; } = [];

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
