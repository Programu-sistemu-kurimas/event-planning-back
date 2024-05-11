using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
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

    private async Task<TaskEntity?> GetEntityById(Guid taskId)
    {
        var taskEntity = await _context.Tasks
            .Include(t => t.AssignedUsers)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId);
        return taskEntity;
    }

    public async Task<Task?> GetById(Guid taskId)
    {
        var taskEntity = await GetEntityById(taskId);
        
        if (taskEntity == null)
            return null;
        var users = taskEntity.AssignedUsers.Select(u =>
            User.Create(u.Id, 
                u.UserName, 
                u.UserSurname, 
                u.PasswordHash, 
                u.Email).User)
            .ToList();
        
        var state = (State)Enum.Parse(typeof(State), taskEntity.TaskState);
        var project = Project.Create(
            taskEntity.Project.Id, 
            taskEntity.Project.ProjectName,
            taskEntity.Project.Description);
        
        var task = Task.Create(
            taskEntity.Id,
            taskEntity.TaskName,
            taskEntity.Description,
            state,
            users,
            project
            );

        return task;

    }

    public async Task<Guid> AddUser(Guid taskId, Guid userId)
    {
        var userEntity = await _context.Users.FindAsync(userId);
        var taskEntity = await GetEntityById(taskId);
        

        if (userEntity == null || taskEntity == null)
            return Guid.Empty;
        
        taskEntity.AssignedUsers.Add(userEntity);
        await _context.SaveChangesAsync();

        return taskEntity.Id;
    }

    public async Task<List<User>?> GetUsers(Guid taskId)
    {
        var taskEntity = await GetEntityById(taskId);

        return taskEntity?.AssignedUsers.Select(u =>
            User.Create(u.Id, 
                u.UserName, 
                u.UserSurname, 
                u.PasswordHash, 
                u.Email).User).ToList();
    }
}