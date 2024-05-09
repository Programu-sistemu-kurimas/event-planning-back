using Event_planning_back.Core.Models;

namespace Event_planning_back.DataAccess.Entities;
public class UserEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public string UserSurname { get;  set; }  = string.Empty;
    
    public string PasswordHash { get;  set; }  = string.Empty;
    public string Email { get;   set; }  = string.Empty;

    public ICollection<ProjectEntity> Projects { get; private set; } = new List<ProjectEntity>();
}