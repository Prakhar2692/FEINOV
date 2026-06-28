namespace Feinov.Application.Common.Interfaces;

public interface IProductService
{
    Task<Guid> CreateProductAsync(
        Guid subcategoryId,
        string productName,
        string? description,
        string? brand,
        CancellationToken cancellationToken = default);
}
