using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public class DatabaseHealthService(Context dbContext) : IDatabaseHealthService
{
    public async Task<bool> CheckDatabaseConnectivityAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbContext.Database.CanConnectAsync(cancellationToken);
        }
        catch
        {
            return false;
        }
    }
}
