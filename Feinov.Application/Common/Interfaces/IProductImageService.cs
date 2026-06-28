using Microsoft.AspNetCore.Http;

namespace Feinov.Application.Common.Interfaces;

public interface IProductImageService
{
    Task<IReadOnlyList<string>> UploadAsync(
        Guid productId,
        IEnumerable<IFormFile> files,
        CancellationToken cancellationToken = default);
}
