using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;

namespace Event_planning_back.Core.Abstractions;

public interface IProjectService
{
    Task<Guid> CreateProject(string projectName, string projectDesc, Guid userId);

    Task<bool> SetUserRole(Guid userAdminId, Guid userId, Guid projectId, Role role);

    Task<Guid> AddUserToProject(string userEmail, Guid projectId);

    Task<Project?> GetById(Guid projectId);

    Task<bool> AsserRole(Guid userId, Guid projectId, Role role);

    Task<Role> GetUserRole(User user, Project project);

    Task<List<Guest>?> GetGuests(Guid projectId);

    Task<bool> DeleteProject(Guid projectId, Guid userId);
}