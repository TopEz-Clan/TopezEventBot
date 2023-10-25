namespace TopezEventBot.Data.Entities;

public class SchedulableEventParticipation
{
    public long EventId { get; set; }
    public SchedulableEvent Event { get; set; }
    public long AccountLinkId { get; set; }
    public AccountLink AccountLink { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public bool Notified { get; set; }
}