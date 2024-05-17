using Event_planning_back.Contracts.Guest;
using Event_planning_back.Contracts.Project;
using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Abstractions;
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

    public ProjectsController(IProjectService projectService, IJwtProvider jwtProvider)
    {
        _projectService = projectService;
        _jwtProvider = jwtProvider;
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

   
    
    
}

