using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;

namespace Event_planning_back.Core.Abstractions;

public interface IProjectService
{
    Task<Guid> CreateProject(string projectName, string projectDesc, Guid userId);

    Task<bool> SetUserRole(Guid userAdminId, Guid userId, Guid projectId, Role role);

    Task<Guid> AddUserToProject(string userEmail, Guid projectId);

    Task<Project?> GetById(Guid userId, Guid projectId);

    Task<Role> GetUserRole(Guid userId, Guid projectId);
}