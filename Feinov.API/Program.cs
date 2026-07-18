using Feinov.API.Endpoints;
using Feinov.API.Middleware;
using Feinov.Application;
using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
    var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "feinov";
    var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "feinov";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        RoleClaimType = "role",
        NameClaimType = JwtRegisteredClaimNames.UniqueName
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

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
app.UseAuthentication();
app.UseAuthorization();

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
