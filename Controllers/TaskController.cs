
using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Event_planning_back.Contracts.Project;
using Task = Event_planning_back.Core.Models.Task;

namespace Event_planning_back.Controllers;


[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;

    public TaskController(ITaskRepository taskRepository, IJwtProvider jwtProvider, IProjectService projectService, ITaskService taskService, IUserService userService)
    {
        _jwtProvider = jwtProvider;
        _projectService = projectService;
        _taskService = taskService;
        _userService = userService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> AddTask(TaskCreateRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        if (!await _projectService.AsserRole(userId, request.ProjectId, Role.Admin) &&
            !await _projectService.AsserRole(userId, request.ProjectId, Role.Owner))
            return Forbid();

        var taskId = await _taskService.CreateTaskToProject(request.ProjectId, request.TaskName, request.TaskDescription);

        if (taskId == Guid.Empty)
            return BadRequest();

        return Ok(taskId);
    }

    [Authorize]
    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTask(Guid taskId)
    {
        var task = await _taskService.GetTaskById(taskId);

        if (task == null)
            return NotFound();
        var userResponsesLists =  task.Users
            .Select(async u =>
            {
                var role = await _projectService.GetUserRole(u, task.Project);
                return new UserResponseList(
                    u.Id, u.UserName, u.UserSurname, u.Email, role.ToString());
            });
        
        var userList = await System.Threading.Tasks.Task.WhenAll(userResponsesLists);
        
        var response = new TaskResponse(
            task.Id, 
            task.TaskName, 
            task.Description, 
            userList.ToList()
        );
        
        return Ok(response);
    }

    [Authorize]
    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUserToTask(AddUserToTaskRequest request)
    {
        var response = await _taskService.AddUserToTask(request.TaskId, request.UserId);
        if (response == Guid.Empty)
            return NotFound();

        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{taskId:guid}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);

        var task = await _taskService.GetTaskById(taskId);
        if (task == null)
            return NotFound();

        if (!await _projectService.AsserRole(userId, task.Project.Id, Role.Admin) &&
            !await _projectService.AsserRole(userId, task.Project.Id, Role.Owner))
            return Forbid();

        if(!await _taskService.DeleteTask(taskId))
            return BadRequest();

        return Ok();

    }
}