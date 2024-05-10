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
        var user = await _userRepository.GetByIdWithProjects(userId);
        if (user == null)
            return Guid.Empty;

        var project = Project.Create(Guid.NewGuid(), projectName, projectDesc);
        var projectId = await _projectRepository.Create(project, user);
        
        await _projectRepository.AddRole(project, user, Role.Admin);
        
        return projectId;
    }

    public async Task<bool> SetUserRole(Guid userAdminId, Guid userId, Guid projectId, Role role)
    {
        var admin =await _userRepository.GetByIdWithoutProjects(userAdminId);
        var user = await _userRepository.GetByIdWithoutProjects(userId);
        var project =await _projectRepository.GetById(projectId);

        if (project == null || user == null || admin == null)
            return false;
        
        if(await _projectRepository.GetRole(project, admin) != Role.Admin)
            return false;
        
        return await _projectRepository.AddRole(project, user, role);
    }

    public async Task<Guid> AddUserToProject(string userEmail, Guid projectId)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        var project = await _projectRepository.GetById(projectId);
       
        if (user == null || project == null)
            return Guid.Empty;
        
        await _projectRepository.AddUser(project, user);
        await _projectRepository.AddRole(project, user, Role.User);

        return projectId;
    }

    public async Task<Project?> GetById(Guid userId, Guid projectId)
    {
        var project = await _projectRepository.GetById(projectId);
        if (project == null)
            return null;
        
        var user = project.Workers.FirstOrDefault(u => u.Id == userId);

        if (user == null)
            return null;

        return project;

    }

    public async Task<Role> GetUserRole(Guid userId, Guid projectId)
    {
        var user = await _userRepository.GetByIdWithoutProjects(userId);
        var project = await _projectRepository.GetById(projectId);
        
        if (user == null || project == null)
            return Role.User;
        
        return await _projectRepository.GetRole(project, user);
    }
}