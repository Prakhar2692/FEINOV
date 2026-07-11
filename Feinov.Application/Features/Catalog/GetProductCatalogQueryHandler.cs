using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Catalog;

public sealed class GetProductCatalogQueryHandler(ICatalogService catalogService)
    : IRequestHandler<GetProductCatalogQuery, PagedCatalogResult>
{
    public Task<PagedCatalogResult> Handle(GetProductCatalogQuery request, CancellationToken cancellationToken)
        => catalogService.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
}
