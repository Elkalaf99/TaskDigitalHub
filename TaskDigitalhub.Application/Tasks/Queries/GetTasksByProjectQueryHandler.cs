using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Common.Models;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Queries;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, PagedResult<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTasksByProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var (totalCount, items) = (
            await _unitOfWork.Tasks.GetCountByProjectIdAsync(request.ProjectId),
            await _unitOfWork.Tasks.GetByProjectIdAsync(
                request.ProjectId,
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortDesc)
        );

        var dtos = _mapper.Map<List<TaskDto>>(items);

        return new PagedResult<TaskDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
