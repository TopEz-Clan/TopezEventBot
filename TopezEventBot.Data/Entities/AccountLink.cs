using System.Collections.ObjectModel;

namespace TopezEventBot.Data.Entities;

public class AccountLink
{
    public long Id { get; set; }
    public ulong DiscordMemberId { get; set; }
    public string RunescapeName { get; set; }

    public Collection<EventParticipation> EventParticipations { get; set; } = new();
    public Collection<Event> Events { get; set; } = new();
}