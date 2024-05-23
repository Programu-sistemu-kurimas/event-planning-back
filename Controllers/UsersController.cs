using Event_planning_back.Contracts;
using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_planning_back.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private readonly IJwtProvider _jwtProvider1;
    private readonly IUserService _userService;

    public UsersController(IJwtProvider jwtProvider1, IUserService userService)
    {
        _jwtProvider1 = jwtProvider1;
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
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
        if (response.Equals(Guid.Empty))
        {
            return Conflict();
        }
        
        return  Ok(response);
        
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var response = await _userService.Login(request.Email, request.Password);

        if (response == null)
        {
            return Unauthorized();
        }
        
        Response.Cookies.Append("AuthToken", response.Token);
        return Ok(response);
    }
    [Authorize]
    [HttpGet("projects")]
    public async Task<ActionResult<List<ProjectListResponse>>> GetUserProjects()
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider1.GetUserId(token);
        var projects = await _userService.GetProjects(userId);

        if (projects == null)
            return NotFound();

        var response = projects.Select(p => new ProjectListResponse(
            p.Id,
            p.ProjectName,
            p.Description));

        return Ok(response);
    }
    [Authorize]
    [HttpGet("archivedProjects")]
    public async Task<ActionResult<List<ProjectListResponse>>> GetUserArchivedProjects()
    {
        var token = Request.Cookies["AuthToken"];
        if (token == null)
            return Unauthorized();
        
        var userId = _jwtProvider1.GetUserId(token);
        var projects = await _userService.GetArchivedProjects(userId);

        if (projects == null)
            return NotFound();

        var response = projects.Select(p => new ProjectListResponse(
            p.Id,
            p.ProjectName,
            p.Description));

        return Ok(response);
    }
    
    
}