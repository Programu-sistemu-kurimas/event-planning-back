using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<Guid> CreateUser(User user);

    Task<Guid> Register(string userName, string userSurname, string password, string email);

    Task<string> Login(string email, string password);
}