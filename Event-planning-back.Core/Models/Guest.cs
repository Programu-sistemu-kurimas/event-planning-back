namespace Event_planning_back.Core.Models;

public class Guest
{

    public const int MaxNameLength= 50;
    public Guid Id { get; private set; }
    
    public string Name {get; private set; }
    
    public string Surname { get; private set; }

    public Project? Project;

    private Guest(Guid id, string name, string surname)
    {
        Id = id;
        Name = name;
        Surname = surname;
    }
    
    private Guest(Guid id, string name, string surname, Project? project)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Project = project;
    }


    public static Guest Create(Guid id, string name, string surname, Project? project = null)
    {
        return project == null ? new Guest(id, name, surname) : new Guest(id, name, surname, project);
    }
    
}