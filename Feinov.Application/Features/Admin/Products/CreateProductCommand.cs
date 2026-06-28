using MediatR;

namespace Feinov.Application.Features.Admin.Products;

public sealed record CreateProductCommand(
    Guid SubcategoryId,
    string ProductName,
    string? Description,
    string? Brand) : IRequest<Guid>;
