using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Common.Interfaces;


public interface IUnitOfWork : IDisposable
{
    IProjectRepository Projects { get; }
    ITaskRepository Tasks { get; }
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
}
