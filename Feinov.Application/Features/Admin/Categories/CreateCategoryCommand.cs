using MediatR;

namespace Feinov.Application.Features.Admin.Categories;

public sealed record CreateCategoryCommand(string CategoryName, string? Description) : IRequest<CategoryDto>;

public sealed record CategoryDto(Guid CategoryId, string CategoryName, string? Description, bool IsActive, DateTime CreatedDate);
