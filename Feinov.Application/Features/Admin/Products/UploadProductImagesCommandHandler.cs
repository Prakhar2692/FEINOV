using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Products;

public sealed class UploadProductImagesCommandHandler(
    IProductRepository productRepository,
    IProductImageService productImageService)
    : IRequestHandler<UploadProductImagesCommand, IReadOnlyList<string>>
{
    public async Task<IReadOnlyList<string>> Handle(UploadProductImagesCommand request, CancellationToken cancellationToken)
    {
        var productExists = await productRepository.ProductExistsAsync(request.ProductId, cancellationToken);
        if (!productExists)
            throw new InvalidOperationException("Product does not exist.");

        return await productImageService.UploadAsync(request.ProductId, request.Files, cancellationToken);
    }
}
