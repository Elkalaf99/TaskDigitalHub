using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly ITasksHubClient _tasksHub;
    private readonly ICacheService _cache;

    public DeleteTaskCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, ITasksHubClient tasksHub, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _tasksHub = tasksHub;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdWithProjectAsync(request.Id);
        if (task == null)
            return false;

        if (!CanManageTask(task))
            throw new ForbiddenException();

        var projectId = task.ProjectId;
        _unitOfWork.Tasks.Delete(task);
        await _unitOfWork.SaveChangesAsync();
        _cache.Remove($"task:{request.Id}");
        _cache.Remove($"project:{projectId}");
        await _tasksHub.TaskDeleted(projectId, request.Id);
        return true;
    }

    private bool CanManageTask(Domain.Entities.TaskItem task) =>
        _currentUser.IsAdmin ||
        (task.Project?.ProjectManagerId == _currentUser.UserId) ||
        (task.AssignedToUserId == _currentUser.UserId);
}
