namespace Event_planning_back.Core.Abstractions;

using Models;

public interface ITaskRepository
{
    Task<Guid> Create(Task task, Project project);
    Task<Guid> AddUser(Guid taskId, Guid userId);

    Task<Task?> GetById(Guid taskId);

    Task<List<User>?> GetUsers(Guid taskId);

    Task<bool> Delete(Guid guestId);
}