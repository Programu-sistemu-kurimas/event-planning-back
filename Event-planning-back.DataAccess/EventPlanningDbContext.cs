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
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<UserProjectEntity> UserProject { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasMany(e => e.Projects)
            .WithMany(e => e.Users)
            .UsingEntity<UserProjectEntity>();
        
        modelBuilder.Entity<UserProjectEntity>().Property(up => up.Role).IsRequired(false);
    }
}