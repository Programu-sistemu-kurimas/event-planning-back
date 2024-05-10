
using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace Event_planning_back.Controllers;


[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IProjectService _projectService;

    public TaskController(ITaskRepository taskRepository, IJwtProvider jwtProvider, IProjectService projectService, ITaskService taskService)
    {
        _jwtProvider = jwtProvider;
        _projectService = projectService;
        _taskService = taskService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> AddTask(TaskCreateRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        if (!await _projectService.AsserRole(userId, request.ProjectId, Role.Admin))
            return Forbid();

        var taskId = await _taskService.CreateTaskToProject(request.ProjectId, request.TaskName, request.TaskDescription);

        if (taskId == Guid.Empty)
            return BadRequest();

        return Ok(taskId);
    }
}