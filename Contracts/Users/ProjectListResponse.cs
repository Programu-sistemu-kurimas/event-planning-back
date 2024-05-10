namespace Event_planning_back.Contracts.Users;

public record ProjectListResponse(
    Guid ProjectId,
    string Name,
    string Description);