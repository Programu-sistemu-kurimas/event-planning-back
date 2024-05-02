using Event_planning_back.Contracts;
using Event_planning_back.Contracts.Users;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var response = await _userService.Register(request.Name, request.Surname, request.Password, request.Email);
        return  Ok(response);
        
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var token = await _userService.Login(request.Email, request.Password);
    
        return Ok(token);
    }    
}