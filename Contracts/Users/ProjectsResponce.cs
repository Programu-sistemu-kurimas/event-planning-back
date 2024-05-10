using Event_planning_back.Contracts.Project;

namespace Event_planning_back.Contracts.Users;

public record ProjectResponse(
    Guid Id,
    string ProjectName,
    string Description,
    List<UserListResponce> Workers);
