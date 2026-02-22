using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdWithProjectAsync(request.Id);
        return task == null ? null : _mapper.Map<TaskDto>(task);
    }
}
