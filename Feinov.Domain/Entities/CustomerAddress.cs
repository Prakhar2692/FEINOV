using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class CustomerAddress : BaseEntity
{
    public Guid UserId { get; set; }

    public string? AddressType { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string? Landmark { get; set; }

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = "India";

    public bool IsDefault { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public User User { get; set; } = null!;
}
