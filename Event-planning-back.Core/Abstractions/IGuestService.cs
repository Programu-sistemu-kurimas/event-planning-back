namespace Event_planning_back.Core.Abstractions;

public interface IGuestService
{
    Task<Guid> AddGuest(Guid projectId, string guestName, string guestSurname);

    Task<Guid> DeleteGuest(Guid guestId);

}