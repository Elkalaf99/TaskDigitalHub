using FluentValidation;
using TaskDigitalhub.Application.Projects.Commands;

namespace TaskDigitalhub.Application.Projects.Validators;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(2000)
            .When(x => x.Dto.Description != null);

        RuleFor(x => x.Dto.EndDate)
            .GreaterThanOrEqualTo(x => x.Dto.StartDate)
            .WithMessage("End date must be on or after start date");

        RuleFor(x => x.Dto.Budget)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Budget cannot be negative");
    }
}
