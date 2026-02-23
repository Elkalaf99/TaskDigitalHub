using AutoMapper;
using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Application.Projects.DTOs;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _cache = cache;
    }

    public async Task<ProjectDto?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdWithTasksAsync(request.Id);
        if (project == null)
            return null;

        if (!_currentUser.IsAdmin && (_currentUser.Role != UserRole.ProjectManager || project.ProjectManagerId != _currentUser.UserId))
            throw new ForbiddenException();

        project.Name = request.Dto.Name;
        project.Description = request.Dto.Description;
        project.StartDate = request.Dto.StartDate;
        project.EndDate = request.Dto.EndDate;
        project.Status = request.Dto.Status;
        project.Budget = request.Dto.Budget;
        project.ProjectManagerId = request.Dto.ProjectManagerId;

        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        _cache.Remove($"project:{request.Id}");
        return _mapper.Map<ProjectDto>(project);
    }
}
