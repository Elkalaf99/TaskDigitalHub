using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdWithTasksAsync(request.Id);
        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }
}
