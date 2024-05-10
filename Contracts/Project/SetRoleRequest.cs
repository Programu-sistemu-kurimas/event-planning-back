namespace Event_planning_back.Contracts.Project;

public record SetRoleRequest(
    Guid UserId,
    Guid ProjectId,
    string Role);