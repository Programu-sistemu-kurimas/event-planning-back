using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Models;
using Task = Event_planning_back.Core.Models.Task;

namespace Event_planning_back.Core.Abstractions;

public interface ITaskService
{
    Task<Guid> CreateTaskToProject(Guid projectId, string taskName, string description);
    Task<Guid> AddUserToTask(Guid taskId, Guid userId);
    Task<Core.Models.Task?> GetTaskById(Guid taskId);
    Task<List<User>?> GetAssignedUsers(Guid taskId);
    Task<bool> DeleteTask(Guid taskId);

    Task<Guid> UpdateTask(Guid id, string? title, string? description, string? state);

    Task<Guid> RemoveUserFromTask(Guid userId, Guid taskId);
}