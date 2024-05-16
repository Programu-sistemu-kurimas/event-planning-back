using Event_planning_back.Core.Models;
using Event_planning_back.Core.Security;
using Task = System.Threading.Tasks.Task;

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

        var project = Project.Create(Guid.NewGuid(), projectName, projectDesc);
        var projectId = await _projectRepository.Create(project, user);
        
        await _projectRepository.AddRole(project, user, Role.Owner);
        
        return projectId;
    }

    public async Task<bool> SetUserRole(Guid userAdminId, Guid userId, Guid projectId, Role role)
    {
        var admin =await _userRepository.GetById(userAdminId);
        var user = await _userRepository.GetById(userId);
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

    public async Task<Project?> GetById(Guid projectId)
    {
        var project = await _projectRepository.GetById(projectId);
        
        return project ?? null;
    }

    public async Task<Role> GetUserRole(User user, Project project)
    {
        return await _projectRepository.GetRole(project, user);
    }

    public async Task<bool> AsserRole(Guid userId, Guid projectId, Role role)
    {
        var user = await _userRepository.GetById(userId);
        var project = await _projectRepository.GetById(projectId);
        if (project == null || user == null)
            return false;
       
        var trueRole = await _projectRepository.GetRole(project, user);
        
        return role == trueRole;
    }

    public async Task<List<Guest>?> GetGuests(Guid projectId)
    {
        return await _projectRepository.GetGuests(projectId);
    }

    public async Task<bool> DeleteProject(Guid projectId, Guid userId)
    {
        return await _projectRepository.Delete(projectId);
    }
}