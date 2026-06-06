using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public User User { get; set; } = null!;
}
