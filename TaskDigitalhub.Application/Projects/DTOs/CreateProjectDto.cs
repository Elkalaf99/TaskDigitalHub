using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.DTOs;

public record CreateProjectDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public ProjectStatus Status { get; init; }
    public decimal Budget { get; init; }
    public int? ProjectManagerId { get; init; }
}
