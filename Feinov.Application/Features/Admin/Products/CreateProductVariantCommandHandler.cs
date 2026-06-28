using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Products;

public sealed class CreateProductVariantCommandHandler(
    IProductRepository productRepository,
    IProductVariantService productVariantService)
    : IRequestHandler<CreateProductVariantCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var productExists = await productRepository.ProductExistsAsync(request.ProductId, cancellationToken);
        if (!productExists)
            throw new InvalidOperationException("Product does not exist.");

        return await productVariantService.CreateVariantAsync(
            request.ProductId,
            request.Sku,
            request.VariantName,
            request.SizeMl,
            request.Mrp,
            request.SellingPrice,
            request.Barcode,
            cancellationToken);
    }
}
