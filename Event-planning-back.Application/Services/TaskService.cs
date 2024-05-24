using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Task = Event_planning_back.Core.Models.Task;

namespace Event_planning_back.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Guid> CreateTaskToProject(Guid projectId, string taskName, string description)
    {
        var project = await _projectRepository.GetById(projectId);

        if (project == null)
            return Guid.Empty;

        var task = Task.Create(Guid.NewGuid(), taskName, description);

        return await _taskRepository.Create(task, project);

    }

    public async Task<Task?> GetTaskById(Guid taskId)
    {
        return await _taskRepository.GetById(taskId);
    }

    public async Task<Guid> AddUserToTask(Guid taskId, Guid userId)
    {
        return await _taskRepository.AddUser(taskId, userId);
    }
    
    public async Task<List<User>?> GetAssignedUsers(Guid taskId)
    {
        return await _taskRepository.GetUsers(taskId);
    }

    public async Task<bool> DeleteTask(Guid taskId)
    {
        
        return await _taskRepository.Delete(taskId);
    }

    public async Task<Guid> UpdateTask(Guid id, string? title, string? description, string? state)
    {
        Guid taskId;
        if (Enum.TryParse(state, out State newState))
        {
            taskId = await _taskRepository.Update(id, title, description, newState);
            return taskId;
        }

        taskId = await _taskRepository.Update(id, title, description, null);
        return taskId;
    }

    public async Task<Guid> RemoveUserFromTask(Guid userId, Guid taskId)
    {
        return await _taskRepository.RemoveUser(userId, taskId);
    }
}