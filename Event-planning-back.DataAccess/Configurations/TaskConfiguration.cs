using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = Event_planning_back.Core.Models.Task;

namespace Event_planning_back.DataAccess.Configurations;

public class TaskConfiguration: IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(t => t.AssignedUsers)
            .IsRequired(false);
        builder.Property(t => t.Description)
            .HasMaxLength(Task.MaxTaskdescLength);

        builder.Property(t => t.TaskName)
            .HasMaxLength(Task.MaxTasknameLength);
    }
}