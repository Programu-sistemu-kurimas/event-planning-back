using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;

public interface IUserService
{
    Task<List<User>> GetAllUsers();

    Task<Guid> Register(string userName, string userSurname, string password, string email);

    Task<LoginUserResponse?> Login(string email, string password);
}