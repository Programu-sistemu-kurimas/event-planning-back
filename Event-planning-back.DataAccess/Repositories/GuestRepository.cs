using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly EventPlanningDbContext _context;

    public GuestRepository(EventPlanningDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Add(Guest guest, Guid projectId)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);

        if (projectEntity == null)
            return Guid.Empty;

        var guestEntity = new GuestEntity
        {
            Id = guest.Id,
            Name = guest.Name,
            Project = projectEntity,
            ProjectId = projectEntity.Id,
            Surname = guest.Surname
        };

        await _context.Guests.AddAsync(guestEntity);
        await _context.SaveChangesAsync();

        return guestEntity.Id;
    }

    public async Task<Guid> Delete(Guid guestId)
    {
        var guestEntity = await _context.Guests.FindAsync(guestId);

        if (guestEntity == null)
            return Guid.Empty;

        _context.Guests.Remove(guestEntity);

        await _context.SaveChangesAsync();

        return guestEntity.Id;
    }

   
    
    
}