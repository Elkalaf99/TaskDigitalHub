using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Projects.DTOs;
using TaskDigitalhub.Domain.Entities;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Dto.Name,
            Description = request.Dto.Description,
            StartDate = request.Dto.StartDate,
            EndDate = request.Dto.EndDate,
            Status = request.Dto.Status,
            Budget = request.Dto.Budget
        };

        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }
}
