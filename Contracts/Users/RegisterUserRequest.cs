using System.ComponentModel.DataAnnotations;

namespace Event_planning_back.Contracts.Users;

public record RegisterUserRequest(
    [Required] string Name,
    [Required] string Surname,
    [Required] string Email,
    [Required] string Password);