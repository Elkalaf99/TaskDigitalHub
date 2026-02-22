using FluentValidation;
using TaskDigitalhub.Application.Tasks.Commands;

namespace TaskDigitalhub.Application.Tasks.Validators;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Dto.ProjectId).GreaterThan(0);
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Task title is required")
            .MaximumLength(200);
        RuleFor(x => x.Dto.AssignedToUserId).GreaterThan(0);
        RuleFor(x => x.Dto.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required");
    }
}
