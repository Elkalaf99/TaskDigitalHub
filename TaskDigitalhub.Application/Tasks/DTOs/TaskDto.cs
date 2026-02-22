using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.DTOs;

public record TaskDto
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int AssignedToUserId { get; init; }
    public string? AssignedToUserName { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime DueDate { get; init; }
    public TaskDigitalhub.Domain.Enums.TaskStatus Status { get; init; }
}
