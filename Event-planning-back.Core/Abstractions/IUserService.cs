using Event_planning_back.Contracts.Users;
using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;

public interface IUserService
{
    Task<List<User>> GetAllUsers();

    Task<Guid> Register(string userName, string userSurname, string password, string email);

    Task<LoginUserResponse?> Login(string email, string password);

    Task<Guid> DeleteUser(Guid Id);
    
    Task<List<Project>?> GetProjects(Guid userId);
    Task<List<Project>?> GetArchivedProjects(Guid userId);
    
    Task<User?> GetUserByEmail(string email);
    Task<User?> UpdateUser(Guid userId, string? email, string? name, string? surname);
}