using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProjectDto?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdWithTasksAsync(request.Id);
        if (project == null)
            return null;

        project.Name = request.Dto.Name;
        project.Description = request.Dto.Description;
        project.StartDate = request.Dto.StartDate;
        project.EndDate = request.Dto.EndDate;
        project.Status = request.Dto.Status;
        project.Budget = request.Dto.Budget;

        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }
}
