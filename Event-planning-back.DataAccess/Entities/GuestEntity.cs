namespace Event_planning_back.DataAccess.Entities;

public class GuestEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    
    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public Guid ProjectId { get; set; } = Guid.Empty;

    public ProjectEntity Project{ get; set; } = new ProjectEntity();


}