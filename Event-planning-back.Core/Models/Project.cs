namespace Event_planning_back.Core.Models;

public class Project
{
    public const int MAX_DESCRIPTION_LENGHT = 350;
    public const int MAX_PROJECTNAME_LENGHT = 100;
    private Project(Guid id, string projectName, string description)
    {
        Id = id;
        ProjectName = projectName;
        Description = description;
        Workers = new List<User>();
    }
    private Project(Guid id, string projectName, string description, List<User> workers)
    {
        Id = id;
        ProjectName = projectName;
        Description = description;
        Workers = workers;
    }
    
    public Guid Id { get; private set; }
    
    public string ProjectName { get; private set; }

    public string Description { get; private set; }

    
    public ICollection<User> Workers { get; private set; }

    public static Project Create(Guid id, string projectName, string description)
    {
        return new Project(id, projectName, description);
    }
    
    public static Project Create(Guid id, string projectName, string description, List<User> workers)
    {
        return new Project(id, projectName, description, workers);
    }
}