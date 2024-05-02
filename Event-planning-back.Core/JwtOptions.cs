namespace Event_planning_back.Core;

public class JwtOptions
{
     public string SecretKey { get; set; } = string.Empty;
     
     public int ExpireHours { get; set; } 
}