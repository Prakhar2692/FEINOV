using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Categories;

public sealed class CreateSubcategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<CreateSubcategoryCommand, SubcategoryDto>
{
    public async Task<SubcategoryDto> Handle(CreateSubcategoryCommand request, CancellationToken cancellationToken)
    {
        // Validate category exists
        if (!await categoryService.CategoryIdExistsAsync(request.CategoryId, cancellationToken))
            throw new InvalidOperationException($"Category does not exist.");

        // Prevent duplicate subcategory name under same category
        if (await categoryService.SubcategoryNameExistsAsync(request.CategoryId, request.SubcategoryName, cancellationToken))
            throw new InvalidOperationException($"Subcategory '{request.SubcategoryName}' already exists in this category.");

        return await categoryService.CreateSubcategoryAsync(request.CategoryId, request.SubcategoryName, request.Description, cancellationToken);
    }
}
