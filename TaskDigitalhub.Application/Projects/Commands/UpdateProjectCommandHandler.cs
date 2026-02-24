using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Projects.DTOs;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;
    private readonly IProjectsHubClient _projectsHub;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, ICacheService cache, IProjectsHubClient projectsHub)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _cache = cache;
        _projectsHub = projectsHub;
    }

    public async Task<ProjectDto?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdWithTasksAsync(request.Id);
        if (project == null)
            return null;

        if (!_currentUser.IsAdmin && (_currentUser.Role != UserRole.ProjectManager || project.ProjectManagerId != _currentUser.UserId))
            throw new ForbiddenException();

        var projectManagerId = request.Dto.ProjectManagerId;
        if (projectManagerId.HasValue && projectManagerId.Value > 0)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(projectManagerId.Value);
            if (user == null)
                throw new BadRequestException($"ProjectManager with Id {projectManagerId} does not exist. Please use a valid user ID or omit ProjectManagerId.");
        }

        project.Name = request.Dto.Name;
        project.Description = request.Dto.Description;
        project.StartDate = request.Dto.StartDate;
        project.EndDate = request.Dto.EndDate;
        project.Status = request.Dto.Status;
        project.Budget = request.Dto.Budget;
        project.ProjectManagerId = projectManagerId is > 0 ? projectManagerId : null;

        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        _cache.Remove($"project:{request.Id}");
        var updated = await _unitOfWork.Projects.GetByIdWithTasksAsync(request.Id);
        var dto = _mapper.Map<ProjectDto>(updated ?? project);
        await _projectsHub.ProjectUpdated(System.Text.Json.JsonSerializer.Serialize(dto));
        return dto;
    }
}
