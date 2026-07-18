using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Feinov.Infrastructure.Repositories;
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
        services.AddHttpClient();
        services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();
        services.AddScoped<IOTPService, OTPService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRazorpayService, RazorpayService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IProductVariantService, ProductVariantService>();
        services.AddScoped<IProductImageService, ProductImageService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IProductReviewService, ProductReviewService>();
        services.AddScoped<IOrderExpirationService, OrderExpirationService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAdminOrderService, AdminOrderService>();
        services.AddScoped<ICustomerAddressService, CustomerAddressService>();
        services.AddScoped<IPaymentVerificationService, PaymentVerificationService>();

        return services;
    }
}
