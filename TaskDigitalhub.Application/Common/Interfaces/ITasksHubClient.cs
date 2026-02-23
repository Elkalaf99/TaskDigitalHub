namespace TaskDigitalhub.Application.Common.Interfaces;

public interface ITasksHubClient
{
    Task TaskCreated(int projectId, string taskJson);
    Task TaskUpdated(int projectId, string taskJson);
    Task TaskDeleted(int projectId, int taskId);
}
