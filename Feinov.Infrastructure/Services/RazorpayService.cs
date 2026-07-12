using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Orders;
using Microsoft.Extensions.Configuration;

namespace Feinov.Infrastructure.Services;

public sealed class RazorpayService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IRazorpayService
{
    public async Task<RazorpayOrderDetails> CreateOrderAsync(
        Guid orderId,
        string orderNumber,
        decimal totalAmount,
        string currency = "INR",
        CancellationToken cancellationToken = default)
    {
        var keyId = configuration["Razorpay:KeyId"];
        var keySecret = configuration["Razorpay:KeySecret"];
        var baseUrl = configuration["Razorpay:BaseUrl"];

        if (string.IsNullOrWhiteSpace(keyId) || string.IsNullOrWhiteSpace(keySecret) || string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("Razorpay configuration is missing.");

        var amountInPaise = (int)Math.Round(totalAmount * 100m);
        var payload = new
        {
            amount = amountInPaise,
            currency,
            receipt = orderNumber,
            notes = new { orderId = orderId.ToString() }
        };

        using var client = httpClientFactory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(baseUrl), "orders"));
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{keyId}:{keySecret}")));
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        using var response = await client.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Razorpay order creation failed with status {(int)response.StatusCode}: {responseBody}");

        using var responseDocument = JsonDocument.Parse(responseBody);
        var root = responseDocument.RootElement;

        var providerOrderId = root.TryGetProperty("id", out var idElement) ? idElement.GetString() : null;
        var responseCurrency = root.TryGetProperty("currency", out var currencyElement) ? currencyElement.GetString() : currency;
        var responseAmount = root.TryGetProperty("amount", out var amountElement) ? amountElement.GetDecimal() : amountInPaise;
        var responseReceipt = root.TryGetProperty("receipt", out var receiptElement) ? receiptElement.GetString() : orderNumber;

        return new RazorpayOrderDetails(
            providerOrderId,
            responseCurrency,
            responseAmount,
            keyId,
            responseReceipt,
            responseBody);
    }
}
