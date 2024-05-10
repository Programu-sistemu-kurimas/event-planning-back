namespace Event_planning_back.Contracts.Project;

public record TaskResponseList(
    Guid Id,
    string TaskName,
    string Description,
    string State
);