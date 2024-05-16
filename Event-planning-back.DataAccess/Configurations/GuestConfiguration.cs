using Event_planning_back.Core.Models;
using Event_planning_back.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event_planning_back.DataAccess.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<GuestEntity>
{
    public void Configure(EntityTypeBuilder<GuestEntity> builder)
    {
        builder.Property(g => g.Name)
            .HasMaxLength(Guest.MaxNameLength)
            .IsRequired();
        builder.Property(g => g.Surname)
            .HasMaxLength(Guest.MaxNameLength);
    }
}