using Event_planning_back.Contracts.Task;

namespace Event_planning_back.Core.Models;

public class Task
{
    public const int MaxTasknameLength = 100;
    public const int MaxTaskdescLength = 500;
    private Task(Guid id, string taskName, string description, ICollection<User> users)
    {
        Id = id;
        TaskName = taskName;
        Description = description;
        Users = users;
    }
    
    private Task(Guid id, string taskName, string description)
    {
        Id = id;
        TaskName = taskName;
        Description = description;
        Users = new List<User>();
    }
    
    private Task(Guid id, string taskName, string description, State state)
    {
        Id = id;
        TaskName = taskName;
        Description = description;
        Users = new List<User>();
        TaskState = state;
    }
    
    private Task(Guid id, string taskName, string description, State state, ICollection<User> users, Project project)
    {
        Id = id;
        TaskName = taskName;
        Description = description;
        Users = users;
        TaskState = state;
        Project = project;
    }

    public Guid Id { get; private set; } 

    public string TaskName { get; private set; } 

    public string Description { get; private set; }
    
    public Project Project { get; private set; }

    public State TaskState { get; private set; } = State.ToDo;

    public ICollection<User> Users { get; private set; }

    public static Task Create(Guid id, string taskName, string description, ICollection<User> users)
    {
        return new Task(id, taskName, description, users);
    }
    public static Task Create(Guid id, string taskName, string description)
    {
        return new Task(id, taskName, description);
    }
    public static Task Create(Guid id, string taskName, string description, State state)
    {
        return new Task(id, taskName, description, state);
    }
    public static Task Create(Guid id, string taskName, string description, State state, ICollection<User> users, Project project)
    {
        return new Task(id, taskName, description, state, users, project);
    }
}