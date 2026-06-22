namespace Feinov.Application.Common.Interfaces;

public interface IDatabaseHealthService
{
    Task<bool> CheckDatabaseConnectivityAsync(CancellationToken cancellationToken = default);
}
