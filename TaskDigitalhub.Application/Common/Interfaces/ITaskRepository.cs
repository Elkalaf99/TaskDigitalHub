using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Common.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem?> GetByIdWithProjectAsync(int id);
    Task<IReadOnlyList<TaskItem>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize, string? sortBy, bool sortDesc);
    Task<int> GetCountByProjectIdAsync(int projectId);
    Task<IReadOnlyList<TaskItem>> GetOverdueTasksAsync();
    Task<TaskItem> AddAsync(TaskItem task);
    void Update(TaskItem task);
    void UpdateRange(IEnumerable<TaskItem> tasks);
    void Delete(TaskItem task);
}
