using Microsoft.AspNetCore.SignalR;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalHub.Hubs;

namespace TaskDigitalHub.Services;

public class TasksHubNotificationService : ITasksHubClient, IProjectsHubClient
{
    private const string ProjectsGroup = "Projects";
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

    public async Task ProjectCreated(string projectJson)
    {
        await _hubContext.Clients.Group(ProjectsGroup)
            .SendAsync("ProjectCreated", projectJson);
    }

    public async Task ProjectUpdated(string projectJson)
    {
        await _hubContext.Clients.Group(ProjectsGroup)
            .SendAsync("ProjectUpdated", projectJson);
    }

    public async Task ProjectDeleted(int projectId)
    {
        await _hubContext.Clients.Group(ProjectsGroup)
            .SendAsync("ProjectDeleted", projectId);
    }
}
