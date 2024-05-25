
using System.ComponentModel.DataAnnotations;

namespace Event_planning_back.DataAccess.Entities;

public class ProjectEntity
{
    public Guid Id { get;  set; } = Guid.Empty;

    public string ProjectName { get;  set; } = string.Empty;

    public string Description { get;  set; } = string.Empty;
    
    public bool IsArchived { get; set; } = false;

    public ICollection<UserEntity> Users { get;  set; } = new List<UserEntity>();

    public ICollection<TaskEntity> Tasks{ get; set; } = new List<TaskEntity>();

    public ICollection<GuestEntity> Guests { get; set; } = new List<GuestEntity>();
    
    [Timestamp]
    public byte[] RowVersion { get; set; }

}