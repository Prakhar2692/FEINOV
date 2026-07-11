using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Cart;

public sealed class GetCartQueryHandler(ICartService cartService)
    : IRequestHandler<GetCartQuery, GetCartResult>
{
    public Task<GetCartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
        => cartService.GetCartAsync(request.UserId, cancellationToken);
}
