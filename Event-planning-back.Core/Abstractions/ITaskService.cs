namespace Event_planning_back.Core.Abstractions;

public interface ITaskService
{
    Task<Guid> CreateTaskToProject(Guid projectId, string taskName, string description);
}