namespace Event_planning_back.Contracts;

public record UserResponse(
    Guid Id,
    string UserName,
    string UserSurname,
    string Email);