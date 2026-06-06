using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class OtpTransaction : BaseEntity
{
    public string MobileNumber { get; set; } = string.Empty;

    public string OtpCode { get; set; } = string.Empty;

    public string Purpose { get; set; } = string.Empty;

    public bool IsUsed { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedDate { get; set; }
}
