namespace Event_planning_back.DataAccess.Configurations;

using Core.Models;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.UserName)
            .HasMaxLength(User.MaxNameLength)
            .IsRequired();

        builder.Property(x => x.UserSurname)
            .HasMaxLength(User.MaxNameLength)
            .IsRequired();
        
        builder.Property(x => x.PasswordHash)
            .IsRequired();
    }
}