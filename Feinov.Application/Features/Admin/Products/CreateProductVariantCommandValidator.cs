using FluentValidation;

namespace Feinov.Application.Features.Admin.Products;

public sealed class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

        RuleFor(x => x.VariantName)
            .NotEmpty().WithMessage("Variant name is required.")
            .MaximumLength(200).WithMessage("Variant name must not exceed 200 characters.");

        RuleFor(x => x.Mrp)
            .GreaterThan(0).WithMessage("MRP must be greater than zero.");

        RuleFor(x => x.SellingPrice)
            .GreaterThan(0).WithMessage("Selling price must be greater than zero.");

        RuleFor(x => x.SizeMl)
            .GreaterThan(0).When(x => x.SizeMl.HasValue)
            .WithMessage("Size must be greater than zero.");

        RuleFor(x => x.Barcode)
            .MaximumLength(100).WithMessage("Barcode must not exceed 100 characters.");
    }
}
