using FluentValidation;
using TaskDigitalhub.Application.Auth.Commands;

namespace TaskDigitalhub.Application.Auth.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Dto.Password).NotEmpty();
    }
}
