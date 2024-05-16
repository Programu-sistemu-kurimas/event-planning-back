namespace Event_planning_back.Contracts.Guest;

public record CreateGuestRequest(
    Guid ProjectId,
    string GuestName,
    string GuestSurname
);