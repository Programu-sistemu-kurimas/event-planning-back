namespace Event_planning_back.Contracts.Guest;

public record GuestResponse(
    Guid Id,
    string GuestName,
    string GuestSurname
);