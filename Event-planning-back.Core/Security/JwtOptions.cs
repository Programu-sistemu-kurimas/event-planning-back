namespace Event_planning_back.Core.Security;

public class JwtOptions
{
     public string SecretKey { get; set; } = string.Empty;
     
     public int ExpireHours { get; set; } 
}