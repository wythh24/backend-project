using FluentValidation;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.Validation;

public class WareValidate : AbstractValidator<WareCreateReq>
{
    public WareValidate()
    {
        RuleFor(e => e.name).NotNull().Length(3, 50).WithMessage("Name must be not empty");
        RuleFor(e => e.Code).NotNull().Length(36).WithMessage("Code must be not empty");
        RuleFor(e => e.description).Length(0, 200).WithMessage("Description limit until 200 characters");
    }
}