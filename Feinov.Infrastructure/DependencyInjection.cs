using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Feinov.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Feinov.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));

        services.AddScoped<DbContext>(provider => provider.GetRequiredService<Context>());
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();
        services.AddScoped<IOTPService, OTPService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
