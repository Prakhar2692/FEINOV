using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Cart;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class CartService(Context dbContext) : ICartService
{
    public async Task<AddToCartResult> AddAsync(
        Guid userId,
        Guid variantId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AnyAsync(x => x.UserId == userId && x.IsActive, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User does not exist.");

        var variantExists = await dbContext.ProductVariants.AnyAsync(x => x.VariantId == variantId && x.IsActive, cancellationToken);
        if (!variantExists)
            throw new InvalidOperationException("Variant does not exist.");

        var inventory = await dbContext.Inventories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.VariantId == variantId, cancellationToken);

        if (inventory == null)
            throw new InvalidOperationException("Inventory record does not exist for the variant.");

        if (inventory.AvailableStock < quantity)
            throw new InvalidOperationException("Insufficient stock available.");

        var cart = await dbContext.Carts
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (cart == null)
        {
            cart = new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var existingCartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cart.CartId && x.VariantId == variantId, cancellationToken);

        if (existingCartItem != null)
        {
            var newQuantity = existingCartItem.Quantity + quantity;
            if (inventory.AvailableStock < newQuantity)
                throw new InvalidOperationException("Insufficient stock available.");

            existingCartItem.Quantity = newQuantity;
            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddToCartResult(cart.CartId, existingCartItem.CartItemId, newQuantity, inventory.AvailableStock);
        }

        var cartItem = new CartItem
        {
            CartItemId = Guid.NewGuid(),
            CartId = cart.CartId,
            VariantId = variantId,
            Quantity = quantity,
            CreatedDate = DateTime.UtcNow
        };

        dbContext.CartItems.Add(cartItem);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddToCartResult(cart.CartId, cartItem.CartItemId, quantity, inventory.AvailableStock);
    }

    public async Task<UpdateCartItemQuantityResult> UpdateQuantityAsync(
        Guid userId,
        Guid cartItemId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AnyAsync(x => x.UserId == userId && x.IsActive, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User does not exist.");

        var cartItem = await dbContext.CartItems
            .Include(x => x.Cart)
            .Include(x => x.Variant)
            .FirstOrDefaultAsync(x => x.CartItemId == cartItemId, cancellationToken);

        if (cartItem == null)
            throw new InvalidOperationException("Cart item does not exist.");

        if (cartItem.Cart.UserId != userId)
            throw new InvalidOperationException("Cart item does not belong to the specified user.");

        var inventory = await dbContext.Inventories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.VariantId == cartItem.VariantId, cancellationToken);

        if (inventory == null)
            throw new InvalidOperationException("Inventory record does not exist for the variant.");

        if (quantity == 0)
        {
            dbContext.CartItems.Remove(cartItem);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateCartItemQuantityResult(cartItem.CartId, cartItem.CartItemId, 0, true, inventory.AvailableStock);
        }

        if (inventory.AvailableStock < quantity)
            throw new InvalidOperationException("Insufficient stock available.");

        cartItem.Quantity = quantity;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateCartItemQuantityResult(cartItem.CartId, cartItem.CartItemId, quantity, false, inventory.AvailableStock);
    }

    public async Task<GetCartResult> GetCartAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var cart = await dbContext.Carts
            .AsNoTracking()
            .Include(x => x.CartItems)
                .ThenInclude(x => x.Variant)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Subcategory)
                            .ThenInclude(x => x.Category)
            .Include(x => x.CartItems)
                .ThenInclude(x => x.Variant)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductImages)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (cart == null)
            return new GetCartResult(Guid.Empty, Array.Empty<CartLineItem>(), 0m, 0m);

        var lineItems = new List<CartLineItem>();
        foreach (var cartItem in cart.CartItems)
        {
            var variant = cartItem.Variant;
            var product = variant.Product;
            var inventory = await dbContext.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.VariantId == variant.VariantId, cancellationToken);

            var unitPrice = variant.Mrp;
            var subtotal = unitPrice * cartItem.Quantity;
            var images = product.ProductImages
                .OrderBy(image => image.DisplayOrder)
                .ThenBy(image => image.CreatedDate)
                .Select(image => image.ImageUrl)
                .ToList();

            lineItems.Add(new CartLineItem(
                cartItem.CartItemId,
                product.ProductId,
                product.ProductName,
                product.Brand,
                product.Subcategory.Category.CategoryName,
                product.Subcategory.SubcategoryName,
                variant.VariantId,
                variant.VariantName,
                variant.Sku,
                cartItem.Quantity,
                unitPrice,
                subtotal,
                images,
                inventory?.AvailableStock ?? 0));
        }

        var subtotalAmount = lineItems.Sum(x => x.Subtotal);
        return new GetCartResult(cart.CartId, lineItems, subtotalAmount, subtotalAmount);
    }
}
