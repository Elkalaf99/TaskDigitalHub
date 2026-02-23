using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Tasks.DTOs;
using TaskDigitalhub.Domain.Entities;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly ITasksHubClient _tasksHub;
    private readonly ICacheService _cache;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, ITasksHubClient tasksHub, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _tasksHub = tasksHub;
        _cache = cache;
    }

    public async Task<TaskDto?> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Dto.ProjectId);
        if (project == null)
            return null;

        if (!_currentUser.IsAdmin && (_currentUser.Role != UserRole.ProjectManager || project.ProjectManagerId != _currentUser.UserId))
            throw new ForbiddenException("Only Admin or ProjectManager of the project can create tasks");

        var user = await _unitOfWork.Users.GetByIdAsync(request.Dto.AssignedToUserId);
        if (user == null)
            return null;

        var task = new TaskItem
        {
            ProjectId = request.Dto.ProjectId,
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            AssignedToUserId = request.Dto.AssignedToUserId,
            Priority = request.Dto.Priority,
            DueDate = request.Dto.DueDate,
            Status = request.Dto.Status
        };

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Tasks.GetByIdWithProjectAsync(task.Id);
        if (created != null)
        {
            var dto = _mapper.Map<TaskDto>(created);
            _cache.Remove($"task:{task.Id}");
            _cache.Remove($"project:{request.Dto.ProjectId}");
            await _tasksHub.TaskCreated(request.Dto.ProjectId, System.Text.Json.JsonSerializer.Serialize(dto));
            return dto;
        }
        return null;
    }
}
