using Event_planning_back.Contracts;
using Event_planning_back.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Event_planning_back.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        var users = await _userService.GetAllUsers();

        var response = users.Select(u => new UserResponse(u.Id, u.UserName, u.UserSurname, u.Email));
        
        return Ok(response);
    }
}