using MediatR;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Commands;

public record CreateProjectCommand(CreateProjectDto Dto) : IRequest<ProjectDto>;
