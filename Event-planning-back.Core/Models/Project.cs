namespace Event_planning_back.Core.Models;

public class Project
{
    public const int MaxDescriptionLenght = 350;
    public const int MaxProjectnameLenght = 100;
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
    private Project(Guid id, string projectName, string description, List<User> workers, List<Task> tasks)
    {
        Id = id;
        ProjectName = projectName;
        Description = description;
        Workers = workers;
        Tasks = tasks;
    }
    
    private Project(Guid id, string projectName, string description, List<User> workers, List<Task> tasks, List<Guest> guests)
    {
        Id = id;
        ProjectName = projectName;
        Description = description;
        Workers = workers;
        Tasks = tasks;
        Guests = guests;
    }
    
    public Guid Id { get; private set; }

    public ICollection<Task> Tasks { get; private set; } = new List<Task>();
    public string ProjectName { get; private set; }

    public string Description { get; private set; }

    
    public ICollection<User> Workers { get; private set; }
    
    public ICollection<Guest> Guests { get; private set; }
    public static Project Create(Guid id, string projectName, string description)
    {
        return new Project(id, projectName, description);
    }
    
    public static Project Create(Guid id, string projectName, string description, List<User> workers)
    {
        return new Project(id, projectName, description, workers);
    }
    public static Project Create(Guid id, string projectName, string description, List<User> workers, List<Task> tasks)
    {
        return new Project(id, projectName, description, workers, tasks);
    }
    public static Project Create(Guid id, string projectName, string description, List<User> workers, List<Task> tasks, List<Guest> guests)
    {
        return new Project(id, projectName, description, workers, tasks, guests);
    }
    
}