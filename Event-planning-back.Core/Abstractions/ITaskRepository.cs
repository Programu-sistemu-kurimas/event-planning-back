using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Models;
using Task = Event_planning_back.Core.Models.Task;

namespace Event_planning_back.Core.Abstractions;


public interface ITaskRepository
{
    Task<Guid> Create(Task task, Project project);
    Task<Guid> AddUser(Guid taskId, Guid userId);

    Task<Task?> GetById(Guid taskId);

    Task<List<User>?> GetUsers(Guid taskId);

    Task<bool> Delete(Guid guestId);

    Task<Guid> Update(Guid id, string? title, string? description, State? state);
    Task<Guid> RemoveUser(Guid userId, Guid taskId);
}