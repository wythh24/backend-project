using FluentValidation;
using productstockingv1.Models.Request;

namespace productstockingv1.Validation;

public class ProductValidate : AbstractValidator<ProductCreateReq>
{
    public ProductValidate()
    {
        RuleFor(e => e.Name).NotNull().Length(3, 50).WithMessage("Name must be not empty");
        RuleFor(e => e.Code).NotNull().Length(3, 36).WithMessage("Code must be not empty");
        RuleFor(e => e.Description).Length(0,200).WithMessage("Description limit until 200 characters");
        RuleFor(e => e.Price).GreaterThan(10).WithMessage("Price must be not empty");
    }
}
//fixme check validate 