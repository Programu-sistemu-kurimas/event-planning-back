namespace Event_planning_back.Contracts.Guest;

public record DeleteGuestRequest(
    Guid GuestId,
    Guid ProjectId);