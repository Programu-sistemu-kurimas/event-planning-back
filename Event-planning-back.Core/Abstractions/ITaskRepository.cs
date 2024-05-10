namespace Event_planning_back.Core.Abstractions;

using Models;

public interface ITaskRepository
{
    Task<Guid> Create(Task task, Project project);
}