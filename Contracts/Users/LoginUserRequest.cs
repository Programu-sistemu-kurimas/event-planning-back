using System.ComponentModel.DataAnnotations;

namespace Event_planning_back.Contracts.Users;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);