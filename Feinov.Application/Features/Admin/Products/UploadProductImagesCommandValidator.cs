using FluentValidation;

namespace Feinov.Application.Features.Admin.Products;

public sealed class UploadProductImagesCommandValidator : AbstractValidator<UploadProductImagesCommand>
{
    public UploadProductImagesCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required.");

        RuleFor(x => x.Files)
            .NotNull().WithMessage("At least one file is required.");

        RuleForEach(x => x.Files!)
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("Each file must contain content.");
    }
}
