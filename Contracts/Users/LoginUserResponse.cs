using Event_planning_back.Core.Models;

namespace Event_planning_back.Contracts.Users;

public record LoginUserResponse(
    Guid Id,
    string Name,
    string Surname,
    string Email,
    string Token);