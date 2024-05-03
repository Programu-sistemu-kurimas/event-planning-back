using Event_planning_back.Contracts;
using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_planning_back.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        var users = await userService.GetAllUsers();

        var response = users.Select(u => new UserResponse(u.Id, u.UserName, u.UserSurname, u.Email));
        
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        
        var response = await userService.Register(request.Name, request.Surname, request.Password, request.Email);
        if (response.Equals(Guid.Empty))
        {
            return Conflict();
        }
        
        return  Ok(response);
        
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var response = await userService.Login(request.Email, request.Password);

        if (response == null)
        {
            return Unauthorized();
        }
        
        Response.Cookies.Append("AuthToken", response.Token);
        return Ok(response);
    }    
}