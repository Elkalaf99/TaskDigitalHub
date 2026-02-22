using MediatR;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.Commands;

public record BulkStatusUpdateCommand(IReadOnlyList<int> TaskIds, TaskDigitalhub.Domain.Enums.TaskStatus NewStatus) : IRequest<int>;
