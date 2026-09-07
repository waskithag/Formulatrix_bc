using FluentValidation;
using NetCoreApp.DTOs;

namespace NetCoreApp.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(10).WithMessage("Username cannot exceed 10 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");
    }
}
