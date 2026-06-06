using Feinov.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Feinov.Infrastructure.Services;

public sealed class LocalFileStorageService(IConfiguration configuration) : IFileStorageService
{
    private string StorageRoot =>
        configuration["FileStorage:LocalPath"]
        ?? Path.Combine(AppContext.BaseDirectory, "uploads");

    public async Task<string> SaveAsync(Stream content, string fileName, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(StorageRoot);

        var safeFileName = Path.GetFileName(fileName);
        var destinationPath = Path.Combine(StorageRoot, $"{Guid.NewGuid():N}_{safeFileName}");

        await using var fileStream = File.Create(destinationPath);
        await content.CopyToAsync(fileStream, cancellationToken);

        return destinationPath;
    }

    public Task<Stream> GetAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The requested file was not found.", filePath);
        }

        Stream stream = File.OpenRead(filePath);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
