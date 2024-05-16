using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess.Entities;


[PrimaryKey(nameof(UsersId), nameof(ProjectsId))]
public class UserProjectEntity
{
    public Guid UsersId { get; set; }
    
    public Guid ProjectsId { get; set; }
    public string Role { get; set; } = string.Empty;
}