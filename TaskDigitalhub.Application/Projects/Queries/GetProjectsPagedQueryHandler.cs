using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Common.Models;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Queries;

public class GetProjectsPagedQueryHandler : IRequestHandler<GetProjectsPagedQuery, PagedResult<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProjectsPagedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProjectDto>> Handle(GetProjectsPagedQuery request, CancellationToken cancellationToken)
    {
        var (totalCount, items) = (
            await _unitOfWork.Projects.GetTotalCountAsync(request.SearchTerm, request.StatusFilter),
            await _unitOfWork.Projects.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortDesc,
                request.SearchTerm,
                request.StatusFilter)
        );

        var dtos = _mapper.Map<List<ProjectDto>>(items);

        return new PagedResult<ProjectDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
