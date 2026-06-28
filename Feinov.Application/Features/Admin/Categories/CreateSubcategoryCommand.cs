using MediatR;

namespace Feinov.Application.Features.Admin.Categories;

public sealed record CreateSubcategoryCommand(Guid CategoryId, string SubcategoryName, string? Description) : IRequest<SubcategoryDto>;

public sealed record SubcategoryDto(Guid SubcategoryId, Guid CategoryId, string SubcategoryName, string? Description, bool IsActive, DateTime CreatedDate);
