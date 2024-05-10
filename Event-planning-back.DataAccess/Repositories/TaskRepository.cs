using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Task = Event_planning_back.Core.Models.Task;

namespace Event_planning_back.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly EventPlanningDbContext _context;

    public TaskRepository(EventPlanningDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Create(Task task, Project project)
    {
        var projectEntity = await _context.Projects.FindAsync(project.Id);

        if (projectEntity == null)
            return Guid.Empty;

        var taskEntity = new TaskEntity
        {
            TaskName = task.TaskName,
            Id = task.Id,
            Description = task.Description,
            ProjectId = project.Id,
            TaskState = task.TaskState.ToString(),
            Project = projectEntity,
            AssignedUsers = new List<UserEntity>()
        };

        await _context.Tasks.AddAsync(taskEntity);
        await _context.SaveChangesAsync();

        return taskEntity.Id;

    } 
}