using FluentValidation;

namespace Feinov.Application.Features.Admin.Orders;

public sealed class GetAdminOrderListQueryValidator : AbstractValidator<GetAdminOrderListQuery>
{
    public GetAdminOrderListQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}
