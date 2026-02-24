namespace TaskDigitalhub.Application.Common.Interfaces;

public interface IProjectsHubClient
{
    Task ProjectCreated(string projectJson);
    Task ProjectUpdated(string projectJson);
    Task ProjectDeleted(int projectId);
}
