using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class Product : BaseEntity
{
    public Guid SubcategoryId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Subcategory Subcategory { get; set; } = null!;

    public ICollection<ProductImage> Images { get; set; } = [];

    public ICollection<ProductVariant> Variants { get; set; } = [];

    public ICollection<ProductReview> Reviews { get; set; } = [];
}
