using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    
    public async Task<bool> Delete(Guid guestId)
    {
        var taskEntity = await _context.Tasks.FindAsync(guestId);
        if (taskEntity == null)
            return false;
        
        _context.Tasks.Remove(taskEntity);
        await _context.SaveChangesAsync();

        return true;

    }


    public async Task<Guid> Update(Guid id, string? title, string? description, State? state)
    {
        var taskEntity = await _context.Tasks.FindAsync(id);
        
        

        if (taskEntity == null)
            return Guid.Empty;

        if (!string.IsNullOrEmpty(title))
            taskEntity.TaskName = title;

        if (!string.IsNullOrEmpty(description))
            taskEntity.Description = description;

        if (!string.IsNullOrEmpty(state.ToString()))
            taskEntity.TaskState = state.ToString();
        
        _context.Tasks.Update(taskEntity);

        await _context.SaveChangesAsync();

        return id;
    }

    public async Task<Guid> RemoveUser(Guid userId, Guid taskId)
    {
        var taskEntity = await _context.Tasks.
            Include(t => t.AssignedUsers)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        var userEntity = taskEntity?.AssignedUsers.FirstOrDefault(u => u.Id == userId);
        
        if (userEntity == null || taskEntity == null)
            return Guid.Empty;
        
        taskEntity.AssignedUsers.Remove(userEntity);

        await _context.SaveChangesAsync();

        return taskId;
    }
}