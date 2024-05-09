using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess;

public class EventPlanningDbContext : DbContext
{
    public EventPlanningDbContext(DbContextOptions<EventPlanningDbContext> options) 
        : base(options)
    {
    }
    
    public DbSet<UserEntity> Users { get; set; }
}