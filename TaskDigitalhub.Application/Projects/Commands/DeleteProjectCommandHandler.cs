using MediatR;
using TaskDigitalhub.Application.Common.Interfaces;

namespace TaskDigitalhub.Application.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (project == null)
            return false;

        _unitOfWork.Projects.Delete(project);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
