using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskDigitalhub.Application.Common.Interfaces;

namespace TaskDigitalhub.Infrastructure.Jobs;

public class OverdueTasksNotificationJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OverdueTasksNotificationJob> _logger;

    public OverdueTasksNotificationJob(IServiceScopeFactory scopeFactory, ILogger<OverdueTasksNotificationJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("OverdueTasksNotificationJob started");

        using var scope = _scopeFactory.CreateScope();
        var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var overdueTasks = await taskRepository.GetOverdueTasksAsync();

        foreach (var task in overdueTasks)
        {
            var assigneeEmail = task.AssignedToUser?.Email;
            if (string.IsNullOrEmpty(assigneeEmail))
                continue;

            var subject = $"Overdue Task: {task.Title}";
            var body = $@"
                <h2>Task Overdue Reminder</h2>
                <p><strong>Task:</strong> {task.Title}</p>
                <p><strong>Project:</strong> {task.Project?.Name ?? "N/A"}</p>
                <p><strong>Due Date:</strong> {task.DueDate:yyyy-MM-dd}</p>
                <p><strong>Priority:</strong> {task.Priority}</p>
                <p><strong>Status:</strong> {task.Status}</p>
                <p>Please update the task status or complete it as soon as possible.</p>";

            try
            {
                await emailService.SendEmailAsync(assigneeEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send overdue notification for task {TaskId} to {Email}", task.Id, assigneeEmail);
            }
        }

        _logger.LogInformation("OverdueTasksNotificationJob completed. Sent {Count} notifications", overdueTasks.Count);
    }
}
