using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event_planning_back.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.UserName)
            .HasMaxLength(User.MAX_NAME_LENGTH)
            .IsRequired();

        builder.Property(x => x.UserSurname)
            .HasMaxLength(User.MAX_NAME_LENGTH)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .IsRequired();
    }
}