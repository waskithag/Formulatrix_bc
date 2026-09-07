using FluentValidation;
using NetCoreApp.DTOs;

namespace NetCoreApp.Validators;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required.")
            .MinimumLength(5).WithMessage("Employee name need at least 5 characters")
            .MaximumLength(20).WithMessage("Employee name cannot exceed 20 characters.");
    }
}

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required.")
            .MinimumLength(5).WithMessage("Employee name need at least 5 characters")
            .MaximumLength(20).WithMessage("Employee name cannot exceed 20 characters.");
    }
}
