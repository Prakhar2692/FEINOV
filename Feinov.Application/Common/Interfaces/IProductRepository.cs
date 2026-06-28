namespace Feinov.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<Guid> CreateAsync(
        Guid subcategoryId,
        string productName,
        string? description,
        string? brand,
        CancellationToken cancellationToken = default);

    Task<bool> ProductExistsAsync(Guid productId, CancellationToken cancellationToken = default);
}
