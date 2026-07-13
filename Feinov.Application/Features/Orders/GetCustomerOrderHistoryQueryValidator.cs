using FluentValidation;

namespace Feinov.Application.Features.Orders;

public sealed class GetCustomerOrderHistoryQueryValidator : AbstractValidator<GetCustomerOrderHistoryQuery>
{
    public GetCustomerOrderHistoryQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");
    }
}
