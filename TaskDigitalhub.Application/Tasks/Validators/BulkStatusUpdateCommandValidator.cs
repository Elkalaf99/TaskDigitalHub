using FluentValidation;
using TaskDigitalhub.Application.Tasks.Commands;

namespace TaskDigitalhub.Application.Tasks.Validators;

public class BulkStatusUpdateCommandValidator : AbstractValidator<BulkStatusUpdateCommand>
{
    public BulkStatusUpdateCommandValidator()
    {
        RuleFor(x => x.TaskIds)
            .NotEmpty()
            .WithMessage("At least one task ID is required");

        RuleForEach(x => x.TaskIds)
            .GreaterThan(0);
    }
}
