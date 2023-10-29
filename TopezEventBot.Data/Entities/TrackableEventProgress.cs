namespace TopezEventBot.Data.Entities;

public class TrackableEventProgress
{
    public Guid Id { get; set; }
    public int Progress { get; set; }
    public DateTime FetchedAt { get; set; }
}