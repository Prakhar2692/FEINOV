using Feinov.Application.Features.Cart;

namespace Feinov.Application.Common.Interfaces;

public interface ICartService
{
    Task<AddToCartResult> AddAsync(
        Guid userId,
        Guid variantId,
        int quantity,
        CancellationToken cancellationToken = default);

    Task<UpdateCartItemQuantityResult> UpdateQuantityAsync(
        Guid userId,
        Guid cartItemId,
        int quantity,
        CancellationToken cancellationToken = default);

    Task<GetCartResult> GetCartAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
