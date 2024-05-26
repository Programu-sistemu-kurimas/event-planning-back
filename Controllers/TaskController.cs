
using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Event_planning_back.Contracts.Project;
using Microsoft.EntityFrameworkCore;
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

        return Created(String.Empty, new {taskId});
    }

    [Authorize]
    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTask(Guid taskId)
    {
        var task = await _taskService.GetTaskById(taskId);

        if (task == null)
            return NotFound();

        var userList = new List<UserResponseList>();

        foreach (var u in task.Users)
        {
            var role = await _projectService.GetUserRole(u, task.Project);
            var userResponse = new UserResponseList(
                u.Id,
                u.UserName,
                u.UserSurname,
                u.Email,
                role.ToString());
            userList.Add(userResponse);
        }

        var response = new TaskResponse(
            task.Id,
            task.TaskName,
            task.Description,
            task.TaskState.ToString(),
            userList
        );

        return Ok(response);
    }

    
    

    [Authorize]
    [HttpPost("addUser")]
    public async Task<IActionResult> AddUserToTask(AddUserToTaskRequest request)
    {
        try
        {
            var response = await _taskService.AddUserToTask(request.TaskId, request.UserId);
            if (response == Guid.Empty)
                return NotFound();
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Conflict(new { message = "This record has been modified by another user" });
        }
       

        return Created();
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

        return NoContent();

    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateTask(UpdateTaskRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        
        if (!await _projectService.AsserRole(userId, request.ProjectId, Role.Admin) &&
            !await _projectService.AsserRole(userId, request.ProjectId, Role.Owner))
            return Forbid();
        try
        {
            var taskId = await _taskService.UpdateTask(request.TaskId, request.TaskName, request.Description, request.State );
        
            return await GetTask(taskId);
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Conflict(new { message = "This record has been modified by another user" });
        }

    }

    [Authorize]
    [HttpPut("removeUser")]
    public async Task<IActionResult> RemoveUserFromTask(RemoveUserFromTaskRequest request)
    {
        try
        {
            var guid = await _taskService.RemoveUserFromTask(request.UserId, request.TaskId);

            return await GetTask(guid);
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Conflict(new { message = "This record has been modified by another user" });
        }
    }
    
    
}