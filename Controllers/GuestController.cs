using Event_planning_back.Contracts.Guest;
using Event_planning_back.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_planning_back.Controllers;
[ApiController]
[Route("[controller]")]
public class GuestController : ControllerBase
{
    private readonly IGuestService _guestService;

    public GuestController(IGuestService guestService)
    {
        _guestService = guestService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> AddNewGuestToProject(CreateGuestRequest request)
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();

        if (await _guestService.AddGuest(request.ProjectId, request.GuestName, request.GuestSurname) == Guid.Empty)
            return BadRequest();

        return Created();
    }
}