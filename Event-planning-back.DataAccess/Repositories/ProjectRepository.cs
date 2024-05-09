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

    public async Task<Guid> Create(Project project)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == project.Author.Id);

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
        var userProject = await _context.UserProject.FindAsync(user.Id, project.Id);

        if (userProject == null)
            return false;
        
        userProject.Role = role.ToString();
        await _context.SaveChangesAsync();
        
        return true;
    }
}

