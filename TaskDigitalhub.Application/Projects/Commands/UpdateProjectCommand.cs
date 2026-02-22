using MediatR;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Commands;

public record UpdateProjectCommand(int Id, UpdateProjectDto Dto) : IRequest<ProjectDto?>;
