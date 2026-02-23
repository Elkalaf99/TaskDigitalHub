using FluentValidation;
using TaskDigitalhub.Application.Tasks.Commands;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.Validators;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Task ID must be greater than zero");

        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Task title is required")
            .MaximumLength(200);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.Dto.Description));

        RuleFor(x => x.Dto.AssignedToUserId)
            .GreaterThan(0).WithMessage("Assigned user ID must be greater than zero");

        RuleFor(x => x.Dto.DueDate)
            .NotEmpty().WithMessage("Due date is required");

        RuleFor(x => x.Dto.Priority)
            .IsInEnum().WithMessage("Invalid task priority");

        RuleFor(x => x.Dto.Status)
            .IsInEnum().WithMessage("Invalid task status");
    }
}
