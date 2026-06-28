using MediatR;

namespace Feinov.Application.Features.Admin.Products;

public sealed record CreateProductVariantCommand(
    Guid ProductId,
    string Sku,
    string VariantName,
    int? SizeMl,
    decimal Mrp,
    decimal SellingPrice,
    string? Barcode) : IRequest<Guid>;
