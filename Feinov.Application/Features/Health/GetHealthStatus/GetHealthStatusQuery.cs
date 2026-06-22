using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Health.GetHealthStatus;

public sealed record GetHealthStatusQuery : IRequest<HealthStatusDto>;

public sealed record HealthStatusDto(string Status, DateTimeOffset CheckedAtUtc);

public sealed class GetHealthStatusQueryHandler(IDateTimeService dateTimeService, IDatabaseHealthService databaseHealthService)
    : IRequestHandler<GetHealthStatusQuery, HealthStatusDto>
{
    public async Task<HealthStatusDto> Handle(GetHealthStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var isConnected = await databaseHealthService.CheckDatabaseConnectivityAsync(cancellationToken);
            var status = isConnected ? "Healthy" : "Unhealthy";
            return new HealthStatusDto(status, dateTimeService.UtcNow);
        }
        catch
        {
            return new HealthStatusDto("Unhealthy", dateTimeService.UtcNow);
        }
    }
}
