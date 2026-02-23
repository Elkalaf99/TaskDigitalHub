using Microsoft.AspNetCore.SignalR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalHub.Hubs;

namespace TaskDigitalHub.Services;

public class TasksHubNotificationService : ITasksHubClient
{
    private readonly IHubContext<TasksHub> _hubContext;

    public TasksHubNotificationService(IHubContext<TasksHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task TaskCreated(int projectId, string taskJson)
    {
        await _hubContext.Clients.Group($"Project_{projectId}")
            .SendAsync("TaskCreated", taskJson);
    }

    public async Task TaskUpdated(int projectId, string taskJson)
    {
        await _hubContext.Clients.Group($"Project_{projectId}")
            .SendAsync("TaskUpdated", taskJson);
    }

    public async Task TaskDeleted(int projectId, int taskId)
    {
        await _hubContext.Clients.Group($"Project_{projectId}")
            .SendAsync("TaskDeleted", taskId);
    }
}
