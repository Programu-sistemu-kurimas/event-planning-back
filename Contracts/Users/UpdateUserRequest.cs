namespace Event_planning_back.Contracts.Users;

public record UpdateUserRequest(
    string? Name,
    string? Surname,
    string? Email);