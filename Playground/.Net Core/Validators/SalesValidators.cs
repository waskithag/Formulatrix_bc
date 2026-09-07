using FluentValidation;
using NetCoreApp.DTOs;

namespace NetCoreApp.Validators;

public class CreateSalesDtoValidator : AbstractValidator<CreateSalesDto>
{
    public CreateSalesDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

        RuleFor(x => x.ProductIds)
            .NotNull().WithMessage("Product IDs list is required.")
            .NotEmpty().WithMessage("At least one product must be selected.");

        RuleForEach(x => x.ProductIds)
            .GreaterThan(0).WithMessage("Product ID must be greater than zero.");
    }
}

public class UpdateSalesDtoValidator : AbstractValidator<UpdateSalesDto>
{
    public UpdateSalesDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

        RuleFor(x => x.ProductIds)
            .NotNull().WithMessage("Product IDs list is required.")
            .NotEmpty().WithMessage("At least one product must be selected.");

        RuleForEach(x => x.ProductIds)
            .GreaterThan(0).WithMessage("Product ID must be greater than zero.");
    }
}
