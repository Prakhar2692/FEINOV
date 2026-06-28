using FluentValidation;

namespace Feinov.Application.Features.Admin.Inventory;

public sealed class GetInventoryListQueryValidator : AbstractValidator<GetInventoryListQuery>
{
    public GetInventoryListQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}
