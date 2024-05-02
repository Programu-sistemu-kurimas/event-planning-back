using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess.Repositories;

public class UserRepository
{
    private readonly EventPlanningDbContext _context;
    
    public UserRepository(EventPlanningDbContext context)
    {
        this._context = context;
    }

    public async Task<List<User>> GetUsers()
    {
        var userEntities = await _context.Users.AsNoTracking().ToListAsync();

        var users = userEntities
            .Select(b => User.Create(b.Id, b.UserName, b.UserSurname, b.PasswordHash, b.Email).User)
            .ToList();
        
        return users;
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
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync()
    }
}