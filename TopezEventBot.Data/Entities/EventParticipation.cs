namespace TopezEventBot.Data.Entities;

public class EventParticipation
{
    public long EventId { get; set; }
    public Event Event { get; set; }
    public long AccountLinkId { get; set; }
    public AccountLink AccountLink { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    
    public int StartingPoint { get; set; }
    public int EndPoint { get; set; }
}