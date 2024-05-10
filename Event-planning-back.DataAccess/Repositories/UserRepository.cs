using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EventPlanningDbContext _context;
    
    public UserRepository(EventPlanningDbContext context)
    {
        this._context = context;
    }

    public async Task<List<User>> Get()
    {
        var userEntities = await _context.Users.AsNoTracking().ToListAsync();

        var users = userEntities
            .Select(u => User.Create(u.Id, u.UserName, u.UserSurname, u.PasswordHash, u.Email).User)
            .ToList();
        
        return users;
    }

    public async Task<User?> GetByIdWithoutProjects(Guid id)
    {
        var userEntity = await _context.Users.FindAsync(id);
       
        if (userEntity == null)
        {
            return null;
        }

        return User.Create(userEntity.Id, userEntity.UserName, userEntity.UserSurname, userEntity.PasswordHash,
            userEntity.Email).User;
    }

    public async Task<User?> GetByIdWithProjects(Guid id)
    {
        var userEntity = await _context.Users
            .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (userEntity == null)
        {
            return null;
        }
        return User.Create(userEntity.Id, userEntity.UserName, userEntity.UserSurname, userEntity.PasswordHash,
            userEntity.Email, userEntity.Projects.
                Select(p => 
                    Project.Create(p.Id, 
                        p.ProjectName, 
                        p.Description))
                .ToList()).User;
    }
    public async Task<Guid> Create(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            UserName = user.UserName,
            UserSurname = user.UserSurname,
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<Guid> Update(Guid id, string userName, string userSurname, string passwordHash, string email)
    {
        var user = await _context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.UserName, u => userName)
                .SetProperty(u => u.UserSurname, u => userSurname)
                .SetProperty(u => u.PasswordHash, u => passwordHash)
                .SetProperty(u => u.Email, u => email));
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<User?> GetByEmail(string email)
    {
        
        var userEntity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity == null)
        {
            return null;
        }
        return User.Create(userEntity.Id, userEntity.UserName, userEntity.UserSurname, userEntity.PasswordHash, userEntity.Email).User;
    }

    public async Task<List<Project>?> GetProjects(Guid userId)
    {
        var userEntity = await _context.Users
            .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (userEntity == null)
            return null;
        var projects = userEntity.Projects.Select(p => Project.Create(
            p.Id,
            p.ProjectName,
            p.Description
        )).ToList();

        return projects;

    }
}