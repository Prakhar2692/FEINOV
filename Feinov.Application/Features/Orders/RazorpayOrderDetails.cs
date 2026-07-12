namespace Feinov.Application.Features.Orders;

public sealed record RazorpayOrderDetails(
    string? OrderId,
    string? Currency,
    decimal Amount,
    string? Key,
    string? Receipt,
    string? GatewayResponseJson);
