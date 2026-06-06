using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class User : BaseEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? MobileNumber { get; set; }

    public bool IsEmailVerified { get; set; }

    public bool IsMobileVerified { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public ICollection<CustomerAddress> Addresses { get; set; } = [];

    public ICollection<Cart> Carts { get; set; } = [];

    public ICollection<Order> Orders { get; set; } = [];

    public ICollection<ProductReview> Reviews { get; set; } = [];
}
