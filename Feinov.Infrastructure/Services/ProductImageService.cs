using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace Feinov.Infrastructure.Services;

public sealed class ProductImageService(
    Context dbContext,
    IFileStorageService fileStorageService) : IProductImageService
{
    public async Task<IReadOnlyList<string>> UploadAsync(
        Guid productId,
        IEnumerable<IFormFile> files,
        CancellationToken cancellationToken = default)
    {
        var imageUrls = new List<string>();
        var imageFiles = files.Where(file => file is { Length: > 0 }).ToList();

        for (var index = 0; index < imageFiles.Count; index++)
        {
            var file = imageFiles[index];
            await using var stream = file.OpenReadStream();
            var savedPath = await fileStorageService.SaveAsync(stream, file.FileName, cancellationToken);

            var image = new ProductImage
            {
                ImageId = Guid.NewGuid(),
                ProductId = productId,
                ImageUrl = savedPath,
                DisplayOrder = index + 1,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.ProductImages.Add(image);
            imageUrls.Add(savedPath);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return imageUrls;
    }
}
