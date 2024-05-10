using Event_planning_back.Contracts.Project;
using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Event_planning_back.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITaskService _taskService;

    public ProjectsController(IProjectService projectService, IJwtProvider jwtProvider, IUserService userService, ITaskRepository taskRepository, ITaskService taskService)
    {
        _projectService = projectService;
        _jwtProvider = jwtProvider;
        _taskService = taskService;
    }
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProject (CreateProjectRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        if (await _projectService.CreateProject(request.ProjectName, request.ProjectDescription, userId) == Guid.Empty)
            return BadRequest();
        
        return Created();
    }

    [Authorize]
    [HttpPost("addUser")]
    public async Task<IActionResult> AddUserToProject(AddUserRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        if (await _projectService.AddUserToProject(request.Email, request.ProjectId) == Guid.Empty)
            return BadRequest();

        return Created();
    }

    [Authorize]
    [HttpPost("setRole")]
    public async Task<IActionResult> SetUserRole(SetRoleRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        
        var userAdminId = _jwtProvider.GetUserId(token);
        var role= (Role)Enum.Parse(typeof(Role), request.Role);

        var response = await _projectService.SetUserRole(userAdminId, request.UserId, request.ProjectId, role);
        
        if (!response)
            return BadRequest();
        
        return Ok();
    }
    
    [Authorize]
    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetProject(Guid projectId)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        
        var project = await _projectService.GetById(projectId);

        
        if (project == null)
            return NotFound();
        
        
        var workersListTasks = project.Workers.Select(w => GetUserListResponseAsync(w, projectId));
        
        var workers = await Task.WhenAll(workersListTasks);
        var tasks = project.Tasks
            .Select(t => new TaskResponseList(t.Id, 
                t.TaskName, 
                t.Description, 
                t.TaskState.ToString()))
            .ToList();
        
        var response = new ProjectResponse
        (
            project.Id,
            project.ProjectName,
            project.Description,
            workers.ToList(),
            tasks
        );

        return Ok(response);
    }
    private async Task<UserResponseList> GetUserListResponseAsync(User user, Guid projectId)
    {
        var role = await _projectService.GetUserRole(user.Id, projectId);
        
        return new UserResponseList
        (
            user.Id,
            user.UserName,
            user.UserSurname,
            user.Email,
            role.ToString()
        );
    }

   
    
    
}

