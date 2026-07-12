using System.Data;
using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Orders;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Feinov.Infrastructure.Services;

public sealed class OrderService(Context dbContext, IRazorpayService razorpayService) : IOrderService
{
    public async Task<CreateOrderResult> CreateAsync(
        Guid userId,
        string fullName,
        string phoneNumber,
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string postalCode,
        string country,
        string? notes,
        CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AnyAsync(x => x.UserId == userId && x.IsActive, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User does not exist.");

        var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        try
        {
            var cart = await dbContext.Carts
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Variant)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.Subcategory)
                                .ThenInclude(x => x.Category)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Variant)
                        .ThenInclude(x => x.VariantDiscounts)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (cart == null || cart.CartItems.Count == 0)
                throw new InvalidOperationException("Cart is empty.");

            var orderItemsToCreate = new List<(CartItem CartItem, ProductVariant Variant, Product Product, decimal UnitPrice, decimal DiscountAmount, decimal LineTotal)>();
            decimal subtotalAmount = 0m;
            decimal discountAmountTotal = 0m;

            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Quantity <= 0)
                    throw new InvalidOperationException("Cart item quantity must be greater than zero.");

                var variant = cartItem.Variant;
                if (variant == null || !variant.IsActive)
                    throw new InvalidOperationException("Variant does not exist or is inactive.");

                var lockedInventory = await LockInventoryRowAsync(variant.VariantId, cancellationToken);
                if (lockedInventory.AvailableStock < cartItem.Quantity)
                    throw new InvalidOperationException($"Insufficient stock available for variant '{variant.Sku}'.");

                await ReserveInventoryAsync(variant.VariantId, cartItem.Quantity, cancellationToken);

                var unitPrice = CalculateSellingPrice(variant);
                var discountAmount = Math.Max(0m, variant.Mrp - unitPrice);
                var lineTotal = unitPrice * cartItem.Quantity;

                subtotalAmount += lineTotal;
                discountAmountTotal += discountAmount * cartItem.Quantity;

                orderItemsToCreate.Add((cartItem, variant, variant.Product, unitPrice, discountAmount, lineTotal));
            }

            var shippingAmount = 0m;
            var taxAmount = 0m;
            var totalAmount = subtotalAmount + shippingAmount + taxAmount - discountAmountTotal;

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = GenerateOrderNumber(),
                CustomerId = userId,
                OrderStatus = "PendingPayment",
                SubtotalAmount = subtotalAmount,
                DiscountAmount = discountAmountTotal,
                ShippingAmount = shippingAmount,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                PaymentStatus = "Pending",
                Notes = notes,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            var razorpayOrder = await razorpayService.CreateOrderAsync(
                order.OrderId,
                order.OrderNumber,
                order.TotalAmount,
                cancellationToken: cancellationToken);

            var paymentTransaction = new PaymentTransaction
            {
                PaymentTransactionId = Guid.NewGuid(),
                OrderId = order.OrderId,
                PaymentProvider = "Razorpay",
                ProviderOrderId = razorpayOrder.OrderId,
                TransactionAmount = order.TotalAmount,
                PaymentMethod = "Razorpay",
                TransactionStatus = "Created",
                GatewayResponse = razorpayOrder.GatewayResponseJson,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.PaymentTransactions.Add(paymentTransaction);
            await dbContext.SaveChangesAsync(cancellationToken);

            foreach (var orderItemData in orderItemsToCreate)
            {
                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    VariantId = orderItemData.Variant.VariantId,
                    ProductName = orderItemData.Product.ProductName,
                    VariantName = orderItemData.Variant.VariantName,
                    Sku = orderItemData.Variant.Sku,
                    Quantity = orderItemData.CartItem.Quantity,
                    UnitPrice = orderItemData.UnitPrice,
                    DiscountAmount = orderItemData.DiscountAmount * orderItemData.CartItem.Quantity,
                    TotalAmount = orderItemData.LineTotal
                };

                dbContext.OrderItems.Add(orderItem);
            }

            var orderAddress = new OrderAddress
            {
                OrderAddressId = Guid.NewGuid(),
                OrderId = order.OrderId,
                FullName = fullName,
                PhoneNumber = phoneNumber,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                State = state,
                PostalCode = postalCode,
                Country = country
            };

            dbContext.OrderAddresses.Add(orderAddress);
            await dbContext.SaveChangesAsync(cancellationToken);

            dbContext.CartItems.RemoveRange(cart.CartItems);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new CreateOrderResult(order.OrderId, order.OrderNumber, order.TotalAmount, order.OrderStatus, razorpayOrder);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<Inventory> LockInventoryRowAsync(Guid variantId, CancellationToken cancellationToken)
    {
        var lockedInventory = await dbContext.Inventories
            .FromSqlInterpolated($"SELECT * FROM inventory WHERE variant_id = {variantId} FOR UPDATE")
            .FirstOrDefaultAsync(cancellationToken);

        if (lockedInventory == null)
            throw new InvalidOperationException("Inventory record does not exist for the variant.");

        return lockedInventory;
    }

    private async Task ReserveInventoryAsync(Guid variantId, int quantity, CancellationToken cancellationToken)
    {
        var affectedRows = await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE inventory SET reserved_stock = reserved_stock + {quantity}, available_stock = available_stock - {quantity}, last_stock_updated = CURRENT_TIMESTAMP WHERE variant_id = {variantId} AND available_stock >= {quantity}",
            cancellationToken);

        if (affectedRows != 1)
            throw new InvalidOperationException("Inventory reservation failed because stock became unavailable.");
    }

    private static decimal CalculateSellingPrice(ProductVariant variant)
    {
        var activeDiscount = variant.VariantDiscounts
            .Where(discount => discount.IsActive == true
                && (!discount.StartDate.HasValue || discount.StartDate <= DateTime.UtcNow)
                && (!discount.EndDate.HasValue || discount.EndDate >= DateTime.UtcNow))
            .OrderByDescending(discount => discount.StartDate)
            .FirstOrDefault();

        var discountPercentage = activeDiscount?.DiscountPercentage ?? 0m;
        return discountPercentage > 0
            ? variant.Mrp - (variant.Mrp * discountPercentage / 100m)
            : variant.Mrp;
    }

    private static string GenerateOrderNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        return $"ORD-{timestamp}-{suffix}";
    }
}
