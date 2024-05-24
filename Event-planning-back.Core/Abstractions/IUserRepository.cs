using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;

public interface IUserRepository
{
    Task<List<User>> Get();
    Task<User?> GetById(Guid id);

    Task<Guid> Create(User user);
    Task<Guid> Update(Guid id, string? userName, string? userSurname, string? email);
    Task<Guid> Delete(Guid id);

    Task<User?> GetByEmail(string email);

    Task<List<Project>?> GetProjects(Guid userId);

    Task<List<Project>?> GetArchivedProjects(Guid userId);
}