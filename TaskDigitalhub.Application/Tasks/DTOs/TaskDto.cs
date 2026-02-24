using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public TaskDigitalhub.Domain.Enums.TaskStatus Status { get; set; }
}
