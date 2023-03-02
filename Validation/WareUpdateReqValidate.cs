using FluentValidation;
using productstockingv1.Models.Request;

namespace productstockingv1.Validation;

public class WareUpdateReqValidate : AbstractValidator<WareUpdateReq>
{
    public WareUpdateReqValidate()
    {
        RuleFor(e => e.Id).NotNull().MaximumLength(36).WithMessage("Id must be filled");
        RuleFor(e => e.Id).NotNull().Length(3, 100).WithMessage("Name must be filled");
    }
}