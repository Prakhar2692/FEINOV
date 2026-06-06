using Feinov.Application.Common.Interfaces;

namespace Feinov.Infrastructure.Services;

public sealed class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
