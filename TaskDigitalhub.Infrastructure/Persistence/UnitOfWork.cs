using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Infrastructure.Persistence.Repositories;

namespace TaskDigitalhub.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProjectManagementDbContext _context;
    private IProjectRepository? _projects;
    private ITaskRepository? _tasks;
    private IUserRepository? _users;

    public UnitOfWork(ProjectManagementDbContext context)
    {
        _context = context;
    }

    public IProjectRepository Projects => _projects ??= new ProjectRepository(_context);
    public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
