using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;
    private readonly IProjectsHubClient _projectsHub;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, ICacheService cache, IProjectsHubClient projectsHub)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _cache = cache;
        _projectsHub = projectsHub;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (project == null)
            return false;

        if (!_currentUser.IsAdmin && (_currentUser.Role != UserRole.ProjectManager || project.ProjectManagerId != _currentUser.UserId))
            throw new ForbiddenException();

        var projectId = project.Id;
        _unitOfWork.Projects.Delete(project);
        await _unitOfWork.SaveChangesAsync();
        _cache.Remove($"project:{projectId}");
        await _projectsHub.ProjectDeleted(projectId);
        return true;
    }
}
