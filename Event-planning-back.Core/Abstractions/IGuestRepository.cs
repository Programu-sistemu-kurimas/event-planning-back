using Event_planning_back.Core.Models;

namespace Event_planning_back.Core.Abstractions;

public interface IGuestRepository
{
    Task<Guid> Add(Guest guest, Guid projectId);
}