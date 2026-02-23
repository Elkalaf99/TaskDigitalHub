using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class BulkStatusUpdateCommandHandler : IRequestHandler<BulkStatusUpdateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public BulkStatusUpdateCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(BulkStatusUpdateCommand request, CancellationToken cancellationToken)
    {
        var updated = 0;
        foreach (var taskId in request.TaskIds)
        {
            var task = await _unitOfWork.Tasks.GetByIdWithProjectAsync(taskId);
            if (task != null)
            {
                if (!CanManageTask(task))
                    throw new ForbiddenException($"No permission to update task {taskId}");

                task.Status = request.NewStatus;
                _unitOfWork.Tasks.Update(task);
                updated++;
            }
        }

        if (updated > 0)
            await _unitOfWork.SaveChangesAsync();

        return updated;
    }

    private bool CanManageTask(Domain.Entities.TaskItem task) =>
        _currentUser.IsAdmin ||
        (task.Project?.ProjectManagerId == _currentUser.UserId) ||
        (task.AssignedToUserId == _currentUser.UserId);
}
