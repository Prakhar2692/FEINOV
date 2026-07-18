using FluentValidation;

namespace Feinov.Application.Features.Customers;

public sealed class GetCustomerAddressesQueryValidator : AbstractValidator<GetCustomerAddressesQuery>
{
    public GetCustomerAddressesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required.");
    }
}
