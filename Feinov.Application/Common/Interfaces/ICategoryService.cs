using Feinov.Application.Features.Admin.Categories;

namespace Feinov.Application.Common.Interfaces;

public interface ICategoryService
{
    Task<bool> CategoryNameExistsAsync(string categoryName, CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateCategoryAsync(string categoryName, string? description, CancellationToken cancellationToken = default);

    Task<bool> CategoryIdExistsAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<bool> SubcategoryNameExistsAsync(Guid categoryId, string subcategoryName, CancellationToken cancellationToken = default);
    Task<SubcategoryDto> CreateSubcategoryAsync(Guid categoryId, string subcategoryName, string? description, CancellationToken cancellationToken = default);
}
