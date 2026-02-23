using FluentValidation;
using TaskDigitalhub.Application.Auth.Commands;

namespace TaskDigitalhub.Application.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Dto.UserName).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6);
    }
}
