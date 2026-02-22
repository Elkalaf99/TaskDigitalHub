using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Tasks.DTOs;
using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDto?> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Dto.ProjectId);
        if (project == null)
            return null;

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
        return created == null ? null : _mapper.Map<TaskDto>(created);
    }
}
