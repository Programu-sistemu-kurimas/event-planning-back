using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
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

    public async Task<User> GetById(Guid id)
    {
        var userEntity = await _context.Users.FindAsync(id);
        return User.Create(userEntity.Id, userEntity.UserName, userEntity.UserSurname, userEntity.PasswordHash,
            userEntity.Email).User;
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
}