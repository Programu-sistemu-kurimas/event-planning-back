using Event_planning_back.Contracts.Task;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess.Repositories;

using Core.Models;
using Entities;

public class ProjectRepository : IProjectRepository
{
    private readonly EventPlanningDbContext _context;
    
    public ProjectRepository(EventPlanningDbContext context)
    {
        this._context = context;
    }

    public async Task<Guid> Create(Project project, User user)
    {
        var userEntity = await _context.Users.FindAsync(user.Id);

        if (userEntity == null)
            return Guid.Empty;
        
        var projectEntity = new ProjectEntity
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
        };
        
        projectEntity.Users.Add(userEntity);
        await _context.AddAsync(projectEntity);
        await _context.SaveChangesAsync();

        return projectEntity.Id;
    }

    public async Task<bool> AddRole(Project project, User user, Role role)
    {
        var userProject = await _context.UserProject
            .FindAsync(user.Id, project.Id);

        if (userProject == null)
            return false;
        
        userProject.Role = role.ToString();
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<Guid> AddUser(Project project, User user)
    {
        var userEntity = await _context.Users.FindAsync(user.Id);
        var projectEntity = await _context.Projects.FindAsync(project.Id);
        
        
        if (userEntity == null || projectEntity == null)
            return Guid.Empty;
        
        projectEntity.Users.Add(userEntity);

        await _context.SaveChangesAsync();

        return projectEntity.Id;
    }

    public async Task<Project?> GetById(Guid id)
    {
        var projectEntity = await _context.Projects
            .Include(p => p.Users)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (projectEntity == null)
            return null;

        var workers = projectEntity.Users.Select(u => User.Create(
            u.Id,
            u.UserName,
            u.UserSurname,
            u.PasswordHash,
            u.Email).User).ToList();

        var tasks = projectEntity.Tasks.Select(t =>
        {
            var state = (State)Enum.Parse(typeof(State), t.TaskState);
            return Task.Create(t.Id, t.TaskName, t.Description, state);
        }).ToList();
        
        return Project.Create(projectEntity.Id, 
            projectEntity.ProjectName, 
            projectEntity.Description, 
            workers,
            tasks);

    }

    public async Task<List<Guest>?> GetGuests(Guid projectId)
    {
        var projectEntity = await _context.Projects
            .Include(p => p.Guests)
            .FirstOrDefaultAsync(p => p.Id == projectId);


        return projectEntity?.Guests.Select(g => 
                Guest.Create(
                    g.Id, 
                    g.Name, 
                    g.Surname))
            .ToList();
    }

    public async Task<Guid> Unarchive(Guid projectId)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);
        
        if (projectEntity == null)
            return Guid.Empty;
        
        projectEntity.IsArchived = false;
        
        await _context.SaveChangesAsync();
        
        return projectEntity.Id;
    }

    public async Task<Role> GetRole(Project project, User user)
    {
        var userProjectEntity = await _context.UserProject.FindAsync(user.Id, project.Id);
        
        if (userProjectEntity == null)
            return Role.User;
        
        var role = (Role)Enum.Parse(typeof(Role), userProjectEntity.Role);
        
        return role;

    }

    public async Task<Guid> Archive(Guid projectId)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);
        
        if (projectEntity == null)
            return Guid.Empty;
        
        projectEntity.IsArchived = true;
        await _context.SaveChangesAsync();

        return projectEntity.Id;
    }
    public async Task<bool> Delete(Guid projectId)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);
        if (projectEntity == null)
            return false;
        
        _context.Projects.Remove(projectEntity);
        await _context.SaveChangesAsync();

        return true;
    }
}

