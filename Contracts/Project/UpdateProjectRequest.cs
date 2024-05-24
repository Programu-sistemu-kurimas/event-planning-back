namespace Event_planning_back.Contracts.Project;

public record UpdateProjectRequest(
    Guid ProjectId,
    string? ProjectName,
    string? Description);