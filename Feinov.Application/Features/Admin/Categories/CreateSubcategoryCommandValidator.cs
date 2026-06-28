using FluentValidation;

namespace Feinov.Application.Features.Admin.Categories;

public sealed class CreateSubcategoryCommandValidator : AbstractValidator<CreateSubcategoryCommand>
{
    public CreateSubcategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.SubcategoryName)
            .NotEmpty().WithMessage("Subcategory name is required.")
            .MaximumLength(200).WithMessage("Subcategory name must not exceed 200 characters.");
    }
}
