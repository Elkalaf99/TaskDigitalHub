using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Common.Interfaces;

/// <summary>
/// Unit of Work pattern - coordinates the work of multiple repositories.
/// Guideline #3: Single Responsibility - transaction boundary management.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IProjectRepository Projects { get; }
    ITaskRepository Tasks { get; }
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
}
