using FluentValidation;

namespace Feinov.Application.Features.Admin.Inventory;

public sealed class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty().WithMessage("Variant is required.");

        RuleFor(x => x.TotalStock)
            .GreaterThanOrEqualTo(0).WithMessage("Total stock cannot be negative.");

        RuleFor(x => x.ReorderLevel)
            .GreaterThanOrEqualTo(0).WithMessage("Reorder level cannot be negative.");
    }
}
