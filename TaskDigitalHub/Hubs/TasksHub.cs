using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskDigitalHub.Hubs;

[Authorize(Policy = "Authenticated")]
public class TasksHub : Hub
{
    public Task JoinProjectGroup(int projectId) => Groups.AddToGroupAsync(Context.ConnectionId, $"Project_{projectId}");

    public Task LeaveProjectGroup(int projectId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Project_{projectId}");

    public Task JoinProjectsGroup() => Groups.AddToGroupAsync(Context.ConnectionId, "Projects");

    public Task LeaveProjectsGroup() => Groups.RemoveFromGroupAsync(Context.ConnectionId, "Projects");
}
