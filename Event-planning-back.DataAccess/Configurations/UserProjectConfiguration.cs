using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event_planning_back.DataAccess.Configurations;

public class UserProjectConfiguration : IEntityTypeConfiguration<UserProjectEntity>
{
    public void Configure(EntityTypeBuilder<UserProjectEntity> builder)
    {
        builder.Property(up => up.Role).IsRequired(false);
    }
}