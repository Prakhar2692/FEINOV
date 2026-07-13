using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Catalog;

public sealed class GetProductDetailsQueryHandler(ICatalogService catalogService)
    : IRequestHandler<GetProductDetailsQuery, ProductDetailsResult>
{
    public Task<ProductDetailsResult> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        => catalogService.GetByIdAsync(request.ProductId, cancellationToken);
}
