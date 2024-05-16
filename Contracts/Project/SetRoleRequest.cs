using System.ComponentModel.DataAnnotations;

namespace Event_planning_back.Contracts.Project;

public record SetRoleRequest(
    [Required] Guid UserId,
    [Required] Guid ProjectId,
    [Required] string Role);