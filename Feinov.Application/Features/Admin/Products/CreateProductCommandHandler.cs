using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Products;

public sealed class CreateProductCommandHandler(
    ICategoryService categoryService,
    IProductService productService)
    : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var subcategoryExists = await categoryService.SubcategoryIdExistsAsync(request.SubcategoryId, cancellationToken);
        if (!subcategoryExists)
            throw new InvalidOperationException("Subcategory does not exist.");

        return await productService.CreateProductAsync(
            request.SubcategoryId,
            request.ProductName,
            request.Description,
            request.Brand,
            cancellationToken);
    }
}
