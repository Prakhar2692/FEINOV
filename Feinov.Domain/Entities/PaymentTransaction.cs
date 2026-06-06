using Feinov.Domain.Common;

namespace Feinov.Domain.Entities;

public class PaymentTransaction : BaseEntity
{
    public Guid OrderId { get; set; }

    public string PaymentProvider { get; set; } = string.Empty;

    public string? ProviderOrderId { get; set; }

    public string? ProviderPaymentId { get; set; }

    public string? ProviderSignature { get; set; }

    public decimal TransactionAmount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string TransactionStatus { get; set; } = string.Empty;

    public string? GatewayResponse { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Order Order { get; set; } = null!;
}
