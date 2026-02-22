using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class BulkStatusUpdateCommandHandler : IRequestHandler<BulkStatusUpdateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public BulkStatusUpdateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(BulkStatusUpdateCommand request, CancellationToken cancellationToken)
    {
        var updated = 0;
        foreach (var taskId in request.TaskIds)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task != null)
            {
                task.Status = request.NewStatus;
                _unitOfWork.Tasks.Update(task);
                updated++;
            }
        }

        if (updated > 0)
            await _unitOfWork.SaveChangesAsync();

        return updated;
    }
}
