namespace Feinov.Application.Common.Interfaces;

public interface IOrderExpirationService
{
    Task ExpirePendingOrdersAsync(CancellationToken cancellationToken = default);
}
