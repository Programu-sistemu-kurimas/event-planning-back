using Event_planning_back.Contracts.Guest;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_planning_back.Controllers;
[ApiController]
[Route("[controller]")]
public class GuestController : ControllerBase
{
    private readonly IGuestService _guestService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IProjectService _projectService;

    public GuestController(IGuestService guestService, IJwtProvider jwtProvider, IProjectService projectService)
    {
        _guestService = guestService;
        _jwtProvider = jwtProvider;
        _projectService = projectService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> AddNewGuestToProject(CreateGuestRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var guestId = await _guestService.AddGuest(request.ProjectId, request.GuestName, request.GuestSurname);
        
        if (guestId == Guid.Empty)
            return BadRequest();

        return Created(string.Empty, new { guestId });
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteGuest(DeleteGuestRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();

        var userId =  _jwtProvider.GetUserId(token);
        
        if(!await _projectService.AsserRole(userId, request.ProjectId, Role.Admin) &&
           !await _projectService.AsserRole(userId, request.ProjectId, Role.Owner))
            return Forbid();
        
        await _guestService.DeleteGuest(request.GuestId);
        
        return NoContent();
    }
}