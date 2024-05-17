using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;

namespace Event_planning_back.Application.Services;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepository;

    public GuestService(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<Guid> AddGuest(Guid projectId, string guestName, string guestSurname)
    {
        var guest = Guest.Create(Guid.NewGuid(), guestName, guestSurname);

        var guestId = await _guestRepository.Add(guest, projectId);

        return guestId;
    }
}