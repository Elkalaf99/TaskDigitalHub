using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.DTOs;

public class BulkStatusUpdateRequest
{
    public IReadOnlyList<int> TaskIds { get; set; } = [];
    public TaskDigitalhub.Domain.Enums.TaskStatus NewStatus { get; set; }
}
