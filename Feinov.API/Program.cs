using Feinov.API.Endpoints;
using Feinov.API.Middleware;
using Feinov.Application;
using Feinov.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();

app.MapHealthEndpoints();

app.Run();
