using System.Data;
using System.Security.Cryptography;
using System.Text;
using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Payments;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Feinov.Infrastructure.Services;

public sealed class PaymentVerificationService(Context dbContext, IConfiguration configuration) : IPaymentVerificationService
{
    public async Task<VerifyPaymentResult> VerifyAsync(
        string razorpayOrderId,
        string razorpayPaymentId,
        string razorpaySignature,
        CancellationToken cancellationToken = default)
    {
        var keySecret = configuration["Razorpay:KeySecret"];
        if (string.IsNullOrWhiteSpace(keySecret))
            throw new InvalidOperationException("Razorpay configuration is missing.");

        var paymentTransaction = await dbContext.PaymentTransactions
            .FirstOrDefaultAsync(x => x.ProviderOrderId == razorpayOrderId, cancellationToken);

        if (paymentTransaction == null)
            throw new InvalidOperationException("Payment transaction not found for the supplied Razorpay order id.");

        var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.OrderId == paymentTransaction.OrderId, cancellationToken);

        if (order == null)
            throw new InvalidOperationException("Order not found for the supplied payment transaction.");

        var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        try
        {
            var expectedSignature = CreateSignature(razorpayOrderId, razorpayPaymentId, keySecret);
            if (!string.Equals(expectedSignature, razorpaySignature, StringComparison.Ordinal))
            {
                var orderItems = await dbContext.OrderItems
                    .Where(x => x.OrderId == order.OrderId)
                    .ToListAsync(cancellationToken);

                foreach (var orderItem in orderItems)
                {
                    var inventory = await dbContext.Inventories
                        .FromSqlInterpolated($"SELECT * FROM inventory WHERE variant_id = {orderItem.VariantId} FOR UPDATE")
                        .FirstOrDefaultAsync(cancellationToken);

                    if (inventory == null)
                        throw new InvalidOperationException($"Inventory record does not exist for variant '{orderItem.VariantId}'.");

                    inventory.ReservedStock = Math.Max(0, inventory.ReservedStock - orderItem.Quantity);
                    inventory.AvailableStock = Math.Max(0, inventory.TotalStock - inventory.ReservedStock);
                    inventory.LastStockUpdated = DateTime.UtcNow;
                }

                paymentTransaction.TransactionStatus = "Failed";
                paymentTransaction.ProviderPaymentId = razorpayPaymentId;
                paymentTransaction.ProviderSignature = razorpaySignature;
                paymentTransaction.UpdatedDate = DateTime.UtcNow;
                paymentTransaction.GatewayResponse = "Signature verification failed.";

                order.OrderStatus = "PaymentFailed";
                order.PaymentStatus = "Failed";
                order.UpdatedDate = DateTime.UtcNow;

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new VerifyPaymentResult(order.OrderId, order.OrderNumber, order.OrderStatus, order.PaymentStatus, false, "Razorpay signature verification failed.");
            }

            var successfulOrderItems = await dbContext.OrderItems
                .Where(x => x.OrderId == order.OrderId)
                .ToListAsync(cancellationToken);

            foreach (var orderItem in successfulOrderItems)
            {
                var inventory = await dbContext.Inventories
                    .FromSqlInterpolated($"SELECT * FROM inventory WHERE variant_id = {orderItem.VariantId} FOR UPDATE")
                    .FirstOrDefaultAsync(cancellationToken);

                if (inventory == null)
                    throw new InvalidOperationException($"Inventory record does not exist for variant '{orderItem.VariantId}'.");

                if (inventory.ReservedStock < orderItem.Quantity)
                    throw new InvalidOperationException("Reserved stock is not available to settle the order.");

                inventory.ReservedStock -= orderItem.Quantity;
                inventory.TotalStock -= orderItem.Quantity;
                inventory.AvailableStock = Math.Max(0, inventory.TotalStock - inventory.ReservedStock);
                inventory.LastStockUpdated = DateTime.UtcNow;
            }

            paymentTransaction.ProviderPaymentId = razorpayPaymentId;
            paymentTransaction.ProviderSignature = razorpaySignature;
            paymentTransaction.TransactionStatus = "Authorized";
            paymentTransaction.PaymentMethod = "Razorpay";
            paymentTransaction.UpdatedDate = DateTime.UtcNow;
            paymentTransaction.GatewayResponse = "Signature verified and payment authorized.";

            order.OrderStatus = "Confirmed";
            order.PaymentStatus = "Paid";
            order.UpdatedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new VerifyPaymentResult(order.OrderId, order.OrderNumber, order.OrderStatus, order.PaymentStatus, true, "Payment verified successfully.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static string CreateSignature(string razorpayOrderId, string razorpayPaymentId, string secret)
    {
        var payload = $"{razorpayOrderId}|{razorpayPaymentId}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
