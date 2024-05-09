using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;

namespace Event_planning_back.Core.Abstractions;

public interface IProjectRepository
{
    Task<Guid> Create(Project project);

    Task<bool> AddRole(Project project, User user, Role role);
}