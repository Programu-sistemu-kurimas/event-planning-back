namespace Event_planning_back.Contracts.Task;

public record AddUserToTaskRequest(
    Guid TaskId,
    Guid UserId);