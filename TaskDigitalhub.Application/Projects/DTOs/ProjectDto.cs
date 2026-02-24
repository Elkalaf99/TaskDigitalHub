using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public decimal Budget { get; set; }
    public int? ProjectManagerId { get; set; }
    public string? ProjectManagerName { get; set; }
    public int TaskCount { get; set; }
}
