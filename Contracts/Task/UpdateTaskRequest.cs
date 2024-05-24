namespace Event_planning_back.Contracts.Task;

public record UpdateTaskRequest(
    Guid TaskId,
    Guid ProjectId,
    string? TaskName,
    string? Description,
    string? State);