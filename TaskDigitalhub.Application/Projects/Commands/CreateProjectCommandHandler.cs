using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Projects.DTOs;
using TaskDigitalhub.Domain.Entities;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IProjectsHubClient _projectsHub;

    public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, IProjectsHubClient projectsHub)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _projectsHub = projectsHub;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAdmin && _currentUser.Role != UserRole.ProjectManager)
            throw new ForbiddenException("Only Admin or ProjectManager can create projects");

        int? projectManagerId = request.Dto.ProjectManagerId;
        if (projectManagerId.HasValue && projectManagerId.Value > 0)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(projectManagerId.Value);
            if (user == null)
                throw new BadRequestException($"ProjectManager with Id {projectManagerId} does not exist. Please use a valid user ID or omit ProjectManagerId.");
        }

        var project = new Project
        {
            Name = request.Dto.Name,
            Description = request.Dto.Description,
            StartDate = request.Dto.StartDate,
            EndDate = request.Dto.EndDate,
            Status = request.Dto.Status,
            Budget = request.Dto.Budget,
            ProjectManagerId = projectManagerId is > 0 ? projectManagerId : null
        };

        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Projects.GetByIdWithTasksAsync(project.Id);
        var dto = _mapper.Map<ProjectDto>(created ?? project);
        await _projectsHub.ProjectCreated(System.Text.Json.JsonSerializer.Serialize(dto));
        return dto;
    }
}
