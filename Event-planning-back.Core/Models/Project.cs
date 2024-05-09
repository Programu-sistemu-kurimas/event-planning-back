namespace Event_planning_back.Core.Models;

public class Project
{
    public const int MAX_DESCRIPTION_LENGHT = 350;
    public const int MAX_PROJECTNAME_LENGHT = 100;
    private Project(Guid id, string projectName, string description, User author)
    {
        Id = id;
        ProjectName = projectName;
        Description = description;
        Author = author;
        Workers = new List<User>();
    }

    public Guid Id { get; private set; }
    
    public string ProjectName { get; private set; }

    public string Description { get; private set; }

    public User Author { get; private set; }
    
    public ICollection<User> Workers { get; private set; }

    public static Project Create(Guid id, string projectName, string description, User author)
    {
        return new Project(id, projectName, description, author);
    }

}