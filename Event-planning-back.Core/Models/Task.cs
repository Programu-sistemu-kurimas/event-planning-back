using Event_planning_back.Contracts.Task;

namespace Event_planning_back.Core.Models;

public class Task
{
    public const int MAX_TASKNAME_LENGTH = 100;
    public const int MAX_TASKDESC_LENGTH = 500;
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

    public Guid Id { get; private set; } 

    public string TaskName { get; private set; } 

    public string Description { get; private set; }

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
}