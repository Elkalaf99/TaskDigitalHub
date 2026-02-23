using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Tasks.DTOs;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly ITasksHubClient _tasksHub;
    private readonly ICacheService _cache;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, ITasksHubClient tasksHub, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _tasksHub = tasksHub;
        _cache = cache;
    }

    public async Task<TaskDto?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdWithProjectAsync(request.Id);
        if (task == null)
            return null;

        if (!CanManageTask(task))
            throw new ForbiddenException();

        task.Title = request.Dto.Title;
        task.Description = request.Dto.Description;
        task.AssignedToUserId = request.Dto.AssignedToUserId;
        task.Priority = request.Dto.Priority;
        task.DueDate = request.Dto.DueDate;
        task.Status = request.Dto.Status;

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Tasks.GetByIdWithProjectAsync(request.Id);
        if (updated != null)
        {
            var dto = _mapper.Map<TaskDto>(updated);
            _cache.Remove($"task:{request.Id}");
            _cache.Remove($"project:{updated.ProjectId}");
            await _tasksHub.TaskUpdated(updated.ProjectId, System.Text.Json.JsonSerializer.Serialize(dto));
            return dto;
        }
        return null;
    }

    private bool CanManageTask(Domain.Entities.TaskItem task) =>
        _currentUser.IsAdmin ||
        (task.Project?.ProjectManagerId == _currentUser.UserId) ||
        (task.AssignedToUserId == _currentUser.UserId);
}
