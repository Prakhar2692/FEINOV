using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Categories;

public sealed class CreateCategoryCommandHandler(ICategoryService categoryService)
    : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await categoryService.CategoryNameExistsAsync(request.CategoryName, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Category '{request.CategoryName}' already exists.");

        return await categoryService.CreateCategoryAsync(request.CategoryName, request.Description, cancellationToken);
    }
}
