using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Common.Interfaces;


public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
    Task<Project?> GetByIdWithTasksAsync(int id);
    Task<IReadOnlyList<Project>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy, bool sortDesc, string? searchTerm, int? statusFilter);
    Task<int> GetTotalCountAsync(string? searchTerm, int? statusFilter);
    Task<Project> AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
}
