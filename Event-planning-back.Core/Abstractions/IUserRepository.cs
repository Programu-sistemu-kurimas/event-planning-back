namespace Event_planning_back.Core.Abstractions;
using Models;

public interface IUserRepository
{
    Task<List<User>> Get();
    Task<User?> GetByIdWithProjects(Guid id);
    
    Task<User?> GetByIdWithoutProjects(Guid id);

    Task<Guid> Create(User user);
    Task<Guid> Update(Guid id, string userName, string userSurname, string passwordHash, string email);
    Task<Guid> Delete(Guid id);

    Task<User?> GetByEmail(string email);

    Task<List<Project>?> GetProjects(Guid userId);
}