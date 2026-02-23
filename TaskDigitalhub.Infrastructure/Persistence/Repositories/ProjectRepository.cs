using Microsoft.EntityFrameworkCore;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Entities;
using TaskDigitalhub.Domain.Enums;
using TaskDigitalhub.Infrastructure.Persistence;

namespace TaskDigitalhub.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectManagementDbContext _context;

    public ProjectRepository(ProjectManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project?> GetByIdWithTasksAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Project>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy, bool sortDesc, string? searchTerm, int? statusFilter)
    {
        var query = _context.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || (p.Description != null && p.Description.ToLower().Contains(term)));
        }

        if (statusFilter.HasValue && Enum.IsDefined(typeof(ProjectStatus), statusFilter.Value))
        {
            query = query.Where(p => (int)p.Status == statusFilter.Value);
        }

        query = (sortBy?.ToLowerInvariant(), sortDesc) switch
        {
            ("startdate", true) => query.OrderByDescending(p => p.StartDate),
            ("startdate", _) => query.OrderBy(p => p.StartDate),
            ("enddate", true) => query.OrderByDescending(p => p.EndDate),
            ("enddate", _) => query.OrderBy(p => p.EndDate),
            ("status", true) => query.OrderByDescending(p => p.Status),
            ("status", _) => query.OrderBy(p => p.Status),
            ("budget", true) => query.OrderByDescending(p => p.Budget),
            ("budget", _) => query.OrderBy(p => p.Budget),
            (_, true) => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name)
        };

        return await query
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm, int? statusFilter)
    {
        var query = _context.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || (p.Description != null && p.Description.ToLower().Contains(term)));
        }

        if (statusFilter.HasValue && Enum.IsDefined(typeof(ProjectStatus), statusFilter.Value))
        {
            query = query.Where(p => (int)p.Status == statusFilter.Value);
        }

        return await query.CountAsync();
    }

    public async Task<Project> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        return project;
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }
}
