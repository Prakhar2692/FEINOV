using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class Subcategory : BaseEntity
{
    public Guid CategoryId { get; set; }

    public string SubcategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = [];
}
