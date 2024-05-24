namespace Event_planning_back.Contracts.Task;

public record RemoveUserFromTaskRequest(
    Guid TaskId,
    Guid UserId);
