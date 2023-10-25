using System.Collections.ObjectModel;

namespace TopezEventBot.Data.Entities;

public class SchedulableEvent : Event<SchedulableEventType>
{
    public DateTimeOffset ScheduledAt { get; set; }
    public Collection<SchedulableEventParticipation> EventParticipations { get; set; } = new();
    
    public string Location { get; set; }
}