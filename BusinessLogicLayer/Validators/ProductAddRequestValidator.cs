
using eCommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace eCommerce.BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public  ProductAddRequestValidator()
    {
        RuleFor(temp => temp.ProductName).NotEmpty().WithMessage("Product Name can't be blank");
        RuleFor(temp => temp.Category).IsInEnum().WithMessage("Category Name can't be blank"); 
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Product Name can't be blank"); 
        RuleFor(x => x.QuantityInStock).GreaterThan(0).WithMessage("Product Name can't be blank"); 
    }
}
