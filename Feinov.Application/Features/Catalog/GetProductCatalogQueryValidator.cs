using FluentValidation;

namespace Feinov.Application.Features.Catalog;

public sealed class GetProductCatalogQueryValidator : AbstractValidator<GetProductCatalogQuery>
{
    public GetProductCatalogQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}
