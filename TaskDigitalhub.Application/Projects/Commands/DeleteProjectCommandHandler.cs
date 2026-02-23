using MediatR;
using TaskDigitalhub.Application.Common.Exceptions;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly ICacheService _cache;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (project == null)
            return false;

        if (!_currentUser.IsAdmin && (_currentUser.Role != UserRole.ProjectManager || project.ProjectManagerId != _currentUser.UserId))
            throw new ForbiddenException();

        _unitOfWork.Projects.Delete(project);
        await _unitOfWork.SaveChangesAsync();
        _cache.Remove($"project:{request.Id}");
        return true;
    }
}
