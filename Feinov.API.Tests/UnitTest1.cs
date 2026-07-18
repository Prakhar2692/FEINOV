using System.Net.Http.Json;
using Feinov.Application.Features.Orders;
using Feinov.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Feinov.API.Tests;

public class OrderHistoryEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OrderHistoryEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrderHistory_ReturnsPagedOrders_ForExistingUser()
    {
        var userId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "OrderHistory_ReturnsPagedOrders")
            .Options;

        using (var seedContext = new Context(options))
        {
            var role = new Role { RoleId = Guid.NewGuid(), RoleName = "Customer", CreatedDate = DateTime.UtcNow };
            seedContext.Roles.Add(role);

            var user = new User
            {
                UserId = userId,
                MobileNumber = "9999999999",
                IsActive = true,
                IsEmailVerified = true,
                IsMobileVerified = true,
                RoleId = role.RoleId,
                CreatedDate = DateTime.UtcNow
            };
            seedContext.Users.Add(user);

            seedContext.Orders.AddRange(
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    OrderNumber = "ORDER-0001",
                    CustomerId = userId,
                    OrderStatus = "Completed",
                    PaymentStatus = "Paid",
                    SubtotalAmount = 10m,
                    DiscountAmount = 0m,
                    ShippingAmount = 0m,
                    TaxAmount = 0m,
                    TotalAmount = 10m,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    OrderNumber = "ORDER-0002",
                    CustomerId = userId,
                    OrderStatus = "Completed",
                    PaymentStatus = "Paid",
                    SubtotalAmount = 20m,
                    DiscountAmount = 0m,
                    ShippingAmount = 0m,
                    TaxAmount = 0m,
                    TotalAmount = 20m,
                    CreatedDate = DateTime.UtcNow
                });

            seedContext.SaveChanges();
        }

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<Context>>();
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll<Context>();

                services.AddDbContext<Context>(options =>
                {
                    options.UseInMemoryDatabase("OrderHistory_ReturnsPagedOrders");
                });
            });
        }).CreateClient();

        var request = new { UserId = userId, PageNumber = 1, PageSize = 10 };
        var response = await client.PostAsJsonAsync("/api/orders/history", request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Request failed ({response.StatusCode}): {content}");
        }

        var result = await response.Content.ReadFromJsonAsync<PagedCustomerOrderHistoryResult>();

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(2, result.Items.Count);
    }
}
