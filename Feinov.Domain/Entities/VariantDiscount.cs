using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class VariantDiscount : BaseEntity
{
    public Guid VariantId { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; } = true;

    public ProductVariant Variant { get; set; } = null!;
}
