namespace Event_planning_back.Core.Abstractions;

using Models;

public interface IJwtProvider
{
    string GenerateToken(User user);
    Guid GetUserId(string token);

}