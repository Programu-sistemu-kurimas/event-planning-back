using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Event_planning_back.Core.Abstractions;

public interface IProjectRepository
{
    Task<Guid> Create(Project project, User user);
    Task<Guid> AddUser(Project project, User user);
    Task<bool> AddRole(Project project, User user, Role role);
    Task<Project?> GetById(Guid id);
    Task<Role> GetRole(Project project, User user);
    Task<List<Guest>?> GetGuests(Guid projectId);
    Task<bool> Delete(Guid projectId);
    Task<Guid> Archive(Guid projectId);

    Task<Guid> Unarchive(Guid projectId);

    Task<bool> DeleteUser(Guid projectId, Guid userId);


}