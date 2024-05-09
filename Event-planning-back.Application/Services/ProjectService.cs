using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;

namespace Event_planning_back.Application.Services;

using Core.Abstractions;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    private readonly IUserRepository _userRepository;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Guid> CreateProject (string projectName, string projectDesc, Guid userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
            return Guid.Empty;

        var project = Project.Create(Guid.NewGuid(), projectName, projectDesc, user);
        var projectId = await _projectRepository.Create(project);
        
        await SetUserRole(project, user, Role.Admin);
        
        return projectId;
    }

    public async Task<bool> SetUserRole(Project project, User user, Role role)
    {
        return await _projectRepository.AddRole(project, user, role);
    }
}