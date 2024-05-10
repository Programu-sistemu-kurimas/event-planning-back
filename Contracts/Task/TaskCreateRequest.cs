using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Event_planning_back.Contracts.Task;

public record TaskCreateRequest(
    
    [Required] Guid ProjectId,
    [Required] string TaskName,
    string? TaskDescription
);