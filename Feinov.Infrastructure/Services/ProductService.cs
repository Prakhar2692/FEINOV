using Feinov.Application.Common.Interfaces;

namespace Feinov.Infrastructure.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public Task<Guid> CreateProductAsync(
        Guid subcategoryId,
        string productName,
        string? description,
        string? brand,
        CancellationToken cancellationToken = default)
        => productRepository.CreateAsync(subcategoryId, productName, description, brand, cancellationToken);
}
