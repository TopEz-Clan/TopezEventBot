namespace TopezEventBot.Data.Entities;

public class SchedulableEvent : Event<SchedulableEventType>
{
    public DateTimeOffset ScheduledAt { get; set; }
}