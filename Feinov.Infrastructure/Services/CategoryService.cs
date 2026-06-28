using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Admin.Categories;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public class CategoryService(Context dbContext) : ICategoryService
{
    public async Task<bool> CategoryNameExistsAsync(string categoryName, CancellationToken cancellationToken = default)
        => await dbContext.Categories.AnyAsync(x => x.CategoryName == categoryName, cancellationToken);

    public async Task<CategoryDto> CreateCategoryAsync(string categoryName, string? description, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var category = new Category
        {
            CategoryId = Guid.NewGuid(),
            CategoryName = categoryName,
            Description = description,
            IsActive = true,
            CreatedDate = now
        };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CategoryDto(category.CategoryId, category.CategoryName, category.Description, category.IsActive, category.CreatedDate);
    }

    public async Task<bool> CategoryIdExistsAsync(Guid categoryId, CancellationToken cancellationToken = default)
        => await dbContext.Categories.AnyAsync(x => x.CategoryId == categoryId, cancellationToken);

    public async Task<bool> SubcategoryNameExistsAsync(Guid categoryId, string subcategoryName, CancellationToken cancellationToken = default)
        => await dbContext.Subcategories.AnyAsync(x => x.CategoryId == categoryId && x.SubcategoryName == subcategoryName, cancellationToken);

    public async Task<SubcategoryDto> CreateSubcategoryAsync(Guid categoryId, string subcategoryName, string? description, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var subcategory = new Subcategory
        {
            SubcategoryId = Guid.NewGuid(),
            CategoryId = categoryId,
            SubcategoryName = subcategoryName,
            Description = description,
            IsActive = true,
            CreatedDate = now
        };
        dbContext.Subcategories.Add(subcategory);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new SubcategoryDto(subcategory.SubcategoryId, subcategory.CategoryId, subcategory.SubcategoryName, subcategory.Description, subcategory.IsActive, subcategory.CreatedDate);
    }
}
