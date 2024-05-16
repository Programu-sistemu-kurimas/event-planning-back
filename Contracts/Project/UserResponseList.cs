namespace Event_planning_back.Contracts.Project;

public record UserResponseList(
    Guid Id,
    string Name,
    string Surname,
    string Email,
    string Role);
