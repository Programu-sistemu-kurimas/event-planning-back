
using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;


public interface IJwtProvider
{
    string GenerateToken(User user);
    Guid GetUserId(string token);

}