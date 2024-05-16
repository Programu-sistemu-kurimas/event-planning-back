using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event_planning_back.DataAccess.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Description)
            .HasMaxLength(Project.MaxDescriptionLenght)
            .IsRequired();

        builder.Property(p => p.ProjectName)
            .HasMaxLength(Project.MaxProjectnameLenght)
            .IsRequired();
        
    }
}