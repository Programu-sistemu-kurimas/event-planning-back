using Event_planning_back.Core.Models;
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
    
    public DbSet<GuestEntity> Guests { get; set; }
    
    public DbSet<TaskEntity> Tasks { get; set; }
    
    public DbSet<UserTaskEntity> UserTask { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
        
        modelBuilder.Entity<ProjectEntity>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
        
        modelBuilder.Entity<TaskEntity>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
        
        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.Projects)
            .WithMany(p => p.Users)
            .UsingEntity<UserProjectEntity>(j => j
                .Property(up => up.Role)
                .IsRequired(false));

        modelBuilder.Entity<TaskEntity>()
            .HasMany(t => t.AssignedUsers)
            .WithMany(u => u.Tasks)
            .UsingEntity<UserTaskEntity>(j => j
                .HasKey(ug => new { ug.AssignedUsersId, ug.TasksId }));

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<TaskEntity>()
            .Property(t => t.Description)
            .IsRequired(false);

        modelBuilder.Entity<GuestEntity>()
            .HasOne(g => g.Project)
            .WithMany(p => p.Guests)
            .HasForeignKey(g => g.ProjectId);
        
        modelBuilder.Entity<ProjectEntity>()
            .Property(p => p.IsArchived)
            .HasDefaultValue(false);
        
       
        
    }
}