using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;

public interface ITaskService
{
    Task<Guid> CreateTaskToProject(Guid projectId, string taskName, string description);
    Task<Guid> AddUserToTask(Guid taskId, Guid userId);
    Task<Core.Models.Task?> GetTaskById(Guid taskId);

    Task<List<User>?> GetAssignedUsers(Guid taskId);
    
    
}