using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class Category : BaseEntity
{
    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public ICollection<Subcategory> Subcategories { get; set; } = [];
}
