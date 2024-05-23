namespace Event_planning_back.Application.Services;

using Contracts.Users;
using Core.Abstractions;
using Core.Models;


public class UsersService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IPasswordHasher _passwordHasher;

    private readonly IJwtProvider _jwtProvider;

    public UsersService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.Get();
    }

    public async Task<Guid> Register(string userName, string userSurname, string password, string email)
    {
        var existingUser = await _userRepository.GetByEmail(email);
        
        if (existingUser != null) 
        {
            return Guid.Empty;
        }
        var hashedPassword = _passwordHasher.Generate(password);
        
        var user = User.Create(Guid.NewGuid(), userName, userSurname, hashedPassword, email).User;

        return await _userRepository.Create(user);
    }

    public async Task<LoginUserResponse?> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.Verify(password, user.PasswordHash);
        
        if (result == false)
        {
            return null;
        }

        var token = _jwtProvider.GenerateToken(user);
        
        return new LoginUserResponse(user.Id, user.UserName, user.UserSurname, user.Email, token);
    }

    public async Task<List<Project>?> GetProjects(Guid userId)
    {
        return await _userRepository.GetProjects(userId);
    }
    
    public async Task<List<Project>?> GetArchivedProjects(Guid userId)
    {
        return await _userRepository.GetArchivedProjects(userId);
    }
}