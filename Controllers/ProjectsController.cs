using Event_planning_back.Contracts.Project;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

}