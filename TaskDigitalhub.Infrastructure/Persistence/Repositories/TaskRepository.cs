using Microsoft.EntityFrameworkCore;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Entities;
using TaskDigitalhub.Infrastructure.Persistence;

namespace TaskDigitalhub.Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ProjectManagementDbContext _context;

    public TaskRepository(ProjectManagementDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem?> GetByIdWithProjectAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IReadOnlyList<TaskItem>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize, string? sortBy, bool sortDesc)
    {
        IQueryable<TaskItem> query = _context.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.AssignedToUser);

        query = (sortBy?.ToLowerInvariant(), sortDesc) switch
        {
            ("title", true) => query.OrderByDescending(t => t.Title),
            ("title", _) => query.OrderBy(t => t.Title),
            ("priority", true) => query.OrderByDescending(t => t.Priority),
            ("priority", _) => query.OrderBy(t => t.Priority),
            ("status", true) => query.OrderByDescending(t => t.Status),
            ("status", _) => query.OrderBy(t => t.Status),
            (_, true) => query.OrderByDescending(t => t.DueDate),
            _ => query.OrderBy(t => t.DueDate)
        };

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetCountByProjectIdAsync(int projectId)
    {
        return await _context.Tasks
            .AsNoTracking()
            .CountAsync(t => t.ProjectId == projectId);
    }

    public async Task<IReadOnlyList<TaskItem>> GetOverdueTasksAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.DueDate < today && t.Status != Domain.Enums.TaskStatus.Completed)
            .Include(t => t.AssignedToUser)
            .Include(t => t.Project)
            .ToListAsync();
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        return task;
    }

    public void Update(TaskItem task)
    {
        _context.Tasks.Update(task);
    }

    public void UpdateRange(IEnumerable<TaskItem> tasks)
    {
        _context.Tasks.UpdateRange(tasks);
    }

    public void Delete(TaskItem task)
    {
        _context.Tasks.Remove(task);
    }
}
