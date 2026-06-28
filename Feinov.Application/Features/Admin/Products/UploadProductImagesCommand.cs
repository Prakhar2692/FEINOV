using Microsoft.AspNetCore.Http;
using MediatR;

namespace Feinov.Application.Features.Admin.Products;

public sealed record UploadProductImagesCommand(
    Guid ProductId,
    IEnumerable<IFormFile> Files) : IRequest<IReadOnlyList<string>>;
