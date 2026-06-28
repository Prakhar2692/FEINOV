using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Repositories;

public sealed class ProductRepository(Context dbContext) : IProductRepository
{
    public async Task<Guid> CreateAsync(
        Guid subcategoryId,
        string productName,
        string? description,
        string? brand,
        CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            SubcategoryId = subcategoryId,
            ProductName = productName,
            Description = description,
            Brand = brand,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product.ProductId;
    }

    public Task<bool> ProductExistsAsync(Guid productId, CancellationToken cancellationToken = default)
        => dbContext.Products.AnyAsync(x => x.ProductId == productId, cancellationToken);
}
