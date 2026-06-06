using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class ProductImage : BaseEntity
{
    public Guid ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; } = 1;

    public DateTime CreatedDate { get; set; }

    public Product Product { get; set; } = null!;
}
