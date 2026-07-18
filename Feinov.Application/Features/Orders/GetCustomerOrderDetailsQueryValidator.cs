using FluentValidation;

namespace Feinov.Application.Features.Orders;

public sealed class GetCustomerOrderDetailsQueryValidator : AbstractValidator<GetCustomerOrderDetailsQuery>
{
    public GetCustomerOrderDetailsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required.");

        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order id is required.");
    }
}
