using FluentValidation;

namespace Feinov.Application.Features.Products;

public sealed class AddProductReviewCommandValidator : AbstractValidator<AddProductReviewCommand>
{
    public AddProductReviewCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product id is required.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
    }
}
