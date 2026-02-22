using FluentValidation;
using TaskDigitalhub.Application.Projects.Commands;

namespace TaskDigitalhub.Application.Projects.Validators;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200);
        RuleFor(x => x.Dto.EndDate)
            .GreaterThanOrEqualTo(x => x.Dto.StartDate)
            .WithMessage("End date must be on or after start date");
        RuleFor(x => x.Dto.Budget).GreaterThanOrEqualTo(0);
    }
}
