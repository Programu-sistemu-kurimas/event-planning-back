using System.ComponentModel.DataAnnotations;

namespace Event_planning_back.Contracts.Project;

public record CreateProjectRequest(
    [Required] string ProjectName,
    [Required]string ProjectDescription
);