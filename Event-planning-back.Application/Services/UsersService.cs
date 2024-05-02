using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;

namespace Event_planning_back.Application.Services;

using Event_planning_back.DataAccess.Repositories;

public class UsersService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.Get();
    }

    public async Task<Guid> CreateUser(User user)
    {
        return await _userRepository.Create(user);
    }
    
}