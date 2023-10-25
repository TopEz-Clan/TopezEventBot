using System.Collections.ObjectModel;

namespace TopezEventBot.Data.Entities;

public class AccountLink
{
    public long Id { get; set; }
    public ulong DiscordMemberId { get; set; }
    public string RunescapeName { get; set; }

    public Collection<SchedulableEventParticipation> SchedulableEventParticipations { get; set; } = new();
    public Collection<TrackableEventParticipation> TrackableEventParticipations { get; set; } = new();
    public Collection<TrackableEvent> TrackableEvents { get; set; } = new();
    public Collection<SchedulableEvent> SchedulableEvents { get; set; } = new();
}