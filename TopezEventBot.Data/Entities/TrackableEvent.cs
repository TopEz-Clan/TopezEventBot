using System.Collections.ObjectModel;

namespace TopezEventBot.Data.Entities;

public class TrackableEvent : Event<TrackableEventType>
{
    public Collection<TrackableEventParticipation> EventParticipations { get; set; } = new();
    public bool IsActive { get; set; }
}