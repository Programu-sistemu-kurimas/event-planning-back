using System.ComponentModel.DataAnnotations;

namespace Event_planning_back.Contracts.Project;

public record AddUserRequest(
    [Required]string Email,
    [Required] Guid ProjectId
);
