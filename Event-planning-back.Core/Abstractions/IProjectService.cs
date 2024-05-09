using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;

namespace Event_planning_back.Core.Abstractions;

public interface IProjectService
{
    Task<Guid> CreateProject(string projectName, string projectDesc, Guid userId);

    Task<bool> SetUserRole(Project project, User user, Role role);
}