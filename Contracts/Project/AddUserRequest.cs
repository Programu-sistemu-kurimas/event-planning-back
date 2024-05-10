namespace Event_planning_back.Contracts.Project;

public record AddUserRequest(
    string Email,
    Guid ProjectId
);
