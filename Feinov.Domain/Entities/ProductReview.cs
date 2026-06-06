using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class ProductReview : BaseEntity
{
    public Guid ProductId { get; set; }

    public Guid UserId { get; set; }

    public int Rating { get; set; }

    public string? ReviewTitle { get; set; }

    public string? ReviewText { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedDate { get; set; }

    public Product Product { get; set; } = null!;

    public User User { get; set; } = null!;
}
