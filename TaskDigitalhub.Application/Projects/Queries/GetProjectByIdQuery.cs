using MediatR;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Queries;

public record GetProjectByIdQuery(
    int Id
    ) : IRequest<ProjectDto?>;
