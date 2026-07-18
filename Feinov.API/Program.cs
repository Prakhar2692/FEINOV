using Feinov.API.Endpoints;
using Feinov.API.Middleware;
using Feinov.Application;
using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection is not configured.");

builder.Services.AddHangfire(configuration =>
{
    configuration.UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
    {
        SchemaName = "hangfire",
        QueuePollInterval = TimeSpan.FromSeconds(15),
        DistributedLockTimeout = TimeSpan.FromMinutes(1)
    });
});

builder.Services.AddHangfireServer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHangfireServer();

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
    app.MapOpenApi();
}

RecurringJob.AddOrUpdate<IOrderExpirationService>(
    "expire-pending-orders",
    service => service.ExpirePendingOrdersAsync(CancellationToken.None),
    Cron.Minutely);

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();

app.MapHealthEndpoints();
app.MapAuthEndpoints();
app.MapAdminEndpoints();
app.MapCatalogEndpoints();
app.MapCartEndpoint();
app.MapOrderEndpoint();
app.MapCheckoutEnpoint();
app.MapPaymentEndpoint();
app.MapCustomerEndpoints();
app.MapProductReviewEndpoints();

app.Run();

public partial class Program { }
