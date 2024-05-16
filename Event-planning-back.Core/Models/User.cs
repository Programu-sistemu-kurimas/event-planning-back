namespace Event_planning_back.Core.Models;
public class User
{
    public const int MaxNameLength = 50;
    private User(Guid id, string userName, string userSurname, string passwordHash, string email)
    {
        Id = id;
        UserName = userName;
        UserSurname = userSurname;
        PasswordHash = passwordHash;
        Email = email;
        Projects = new List<Project>();
    }
    private User(Guid id, string userName, string userSurname, string passwordHash, string email, List<Project> projects)
    {
        Id = id;
        UserName = userName;
        UserSurname = userSurname;
        PasswordHash = passwordHash;
        Email = email;
        Projects = projects;
    }

    public Guid Id { get; private set; }
    
    public ICollection<Project> Projects { get; private set; }
    public string UserName { get; private set; }
    
    public string UserSurname { get; private set; }

    
    public string PasswordHash { get; private set; }
    
    public string Email { get; private set; }


    public static (User User, string Error) Create(Guid id, string userName, string userSurname, string passwordHash, string email)
    {

        var error = string.Empty;
        if (string.IsNullOrEmpty(email)) // Add more validations if necessary
        {
            error = "Email must not be empty"; 
        }
        var user =  new User(id, userName, userSurname, passwordHash, email);

        return (user, error);

    }
    public static (User User, string Error) Create(Guid id, string userName, string userSurname, string passwordHash, string email, List<Project> projects)
    {

        var error = string.Empty;
        if (string.IsNullOrEmpty(email)) // Add more validations if necessary
        {
            error = "Email must not be empty"; 
        }
        var user =  new User(id, userName, userSurname, passwordHash, email, projects);

        return (user, error);

    }
    
    
}