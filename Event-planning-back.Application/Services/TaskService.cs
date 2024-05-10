using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
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
}