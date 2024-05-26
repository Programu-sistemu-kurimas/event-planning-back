using Event_planning_back.Contracts.Guest;
using Event_planning_back.Contracts.Project;
using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

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
        var projectId = await _projectService.CreateProject(request.ProjectName, request.ProjectDescription, userId);
        if (projectId == Guid.Empty)
            return BadRequest();

        return Created(string.Empty, new { projectId });
    }

    [Authorize]
    [HttpPost("addUser")]
    public async Task<IActionResult> AddUserToProject(AddUserRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        if (await _projectService.AddUserToProject(request.Email, request.ProjectId) == Guid.Empty)
            return BadRequest();

        return Created();
    }

    [Authorize]
    [HttpPost("archive/{projectId:guid}")]
    public async Task<IActionResult> ArchiveProject(Guid projectId)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);

        if (!await _projectService.AsserRole(userId, projectId, Role.Owner))
            return Forbid();
        
        if (await _projectService.ArchiveProject(projectId) == Guid.Empty)
            return NotFound();
        
        return Ok(projectId);
    }
    
    [Authorize]
    [HttpPost("unarchive/{projectId:guid}")]
    public async Task<IActionResult> UnarchiveProject(Guid projectId)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);

        if (!await _projectService.AsserRole(userId, projectId, Role.Owner))
            return Forbid();
        
        if (await _projectService.UnarchiveProject(projectId) == Guid.Empty)
            return NotFound();
        
        return Ok(projectId);
    }
    [Authorize]
    [HttpPost("setRole")]
    public async Task<IActionResult> SetUserRole(SetRoleRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        
        var userAdminId = _jwtProvider.GetUserId(token);
        
        if(!await _projectService.AsserRole(userAdminId, request.ProjectId, Role.Admin) &&
           !await _projectService.AsserRole(userAdminId, request.ProjectId, Role.Owner))
            return Forbid();

        var role = (Role)Enum.Parse(typeof(Role), request.Role);

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
        
        var project = await _projectService.GetById(projectId);

        if (project == null)
            return NotFound();
        
        var workersList = await Task.WhenAll(project.Workers.Select( async w => 
        {
            var role = await _projectService.GetUserRole(w, project);
            return new UserResponseList
            (
                w.Id,
                w.UserName,
                w.UserSurname,
                w.Email,
                role.ToString()
            );
        }));
        
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
            workersList.ToList(),
            tasks
        );

        return Ok(response);
    }

    [Authorize]
    [HttpGet("guests")]
    public async Task<IActionResult> GetGuests(Guid projectId)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);
        
        if(!await _projectService.AsserRole(userId, projectId, Role.Admin) &&
           !await _projectService.AsserRole(userId, projectId, Role.Owner) &&
           !await _projectService.AsserRole(userId, projectId, Role.User))
            return Forbid();
        
        var guestsList = await _projectService.GetGuests(projectId);
       
        if (guestsList == null)
            return NotFound();
        
        
        
        var response = guestsList.Select(g => new GuestResponse(g.Id,
            g.Name,
            g.Surname)).ToList();
        
        return Ok(response);

    }

    [Authorize]
    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        var userId = _jwtProvider.GetUserId(token);

        if (!await _projectService.AsserRole(userId, projectId, Role.Owner))
            return Forbid();
        
        
        if (!await _projectService.DeleteProject(projectId, userId))
            return NotFound();
        
        return NoContent();

    }

    [Authorize]
    [HttpPut("deleteUser")]
    public async Task<IActionResult> DeleteUserFromProject(AddUserRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        var adminUserId = _jwtProvider.GetUserId(token);

        var userId = (await _userService.GetUserByEmail(request.Email))!.Id;
        if(!await _projectService.AsserRole(adminUserId, request.ProjectId, Role.Admin) &&
           !await _projectService.AsserRole(adminUserId, request.ProjectId, Role.Owner) &&
           adminUserId != userId)
            return Forbid();
        try
        {
            if (!await _projectService.DeleteUserFromProject(request.ProjectId, userId))
                return BadRequest();
            return Ok();
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Conflict(new { message = "This record has been modified by another user" });
        }

        


    }
    
    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateProject(UpdateProjectRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider.GetUserId(token);

        if(!await _projectService.AsserRole(userId, request.ProjectId, Role.Admin) &&
           !await _projectService.AsserRole(userId, request.ProjectId, Role.Owner))
            return Forbid();

        try
        {
            var projectId =
                await _projectService.UpdateProject(request.ProjectId, request.ProjectName, request.Description);
            if (projectId == Guid.Empty)
                return NotFound();
        
            return await GetProject(projectId);
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Conflict(new { message = "This record has been modified by another user" });
        }

    }

   
    
    
}

