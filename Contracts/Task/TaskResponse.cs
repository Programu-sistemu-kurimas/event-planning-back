using Event_planning_back.Contracts.Project;

namespace Event_planning_back.Contracts.Task;

public record TaskResponse(
    Guid Id,
    string TaskName,
    string? TaskDescription,
    string State,
    List<UserResponseList> AssignedUsers);