using eCommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace eCommerce.BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(temp => temp.ProductID).NotEmpty().WithMessage("ProductID can't be blank");
        RuleFor(temp => temp.ProductName).NotEmpty().WithMessage("Product Name can't be blank");
        RuleFor(temp => temp.Category).IsInEnum().WithMessage("Categoryt Name can't be blank");
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Product Name can't be blank");
        RuleFor(x => x.QuantityInStock).GreaterThan(0).WithMessage("Product Name can't be blank");
    }
}

