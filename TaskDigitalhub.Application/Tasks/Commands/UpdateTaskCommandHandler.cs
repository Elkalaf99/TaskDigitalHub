using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDto?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdWithProjectAsync(request.Id);
        if (task == null)
            return null;

        task.Title = request.Dto.Title;
        task.Description = request.Dto.Description;
        task.AssignedToUserId = request.Dto.AssignedToUserId;
        task.Priority = request.Dto.Priority;
        task.DueDate = request.Dto.DueDate;
        task.Status = request.Dto.Status;

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Tasks.GetByIdWithProjectAsync(request.Id);
        return updated == null ? null : _mapper.Map<TaskDto>(updated);
    }
}
