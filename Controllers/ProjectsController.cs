using Event_planning_back.Contracts.Project;
using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserService _userService;

    public ProjectsController(IProjectService projectService, IJwtProvider jwtProvider, IUserService userService)
    {
        _projectService = projectService;
        _jwtProvider = jwtProvider;
        _userService = userService;
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
        
        var project = await _projectService.GetById(userId, projectId);

        
        if (project == null)
            return NotFound();
        
        
        var workersListTasks = project.Workers.Select(w => GetUserListResponseAsync(w, projectId));
        var workers = await Task.WhenAll(workersListTasks);
        var response = new ProjectResponse
        (
            project.Id,
            project.ProjectName,
            project.Description,
            workers.ToList()
        );

        return Ok(response);
    }
    private async Task<UserListResponce> GetUserListResponseAsync(User user, Guid projectId)
    {
        var role = await _projectService.GetUserRole(user.Id, projectId);
        
        return new UserListResponce
        (
            user.Id,
            user.UserName,
            user.UserSurname,
            user.Email,
            role.ToString()
        );
    }
    
}

