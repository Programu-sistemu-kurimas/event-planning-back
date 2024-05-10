using Microsoft.EntityFrameworkCore;

namespace Event_planning_back.DataAccess.Entities;


[PrimaryKey(nameof(AssignedUsersId), nameof(TasksId))]
public class UserTaskEntity
{
    public Guid AssignedUsersId { get; set; }
    public Guid TasksId { get; set; }
}