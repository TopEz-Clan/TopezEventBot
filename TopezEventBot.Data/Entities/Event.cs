using System.Collections.ObjectModel;
using TopezEventBot.Util;

namespace TopezEventBot.Data.Entities;

public class Event
{
    public long Id { get; set; }
    public EventType Type { get; set; }
    public Collection<EventParticipation> EventParticipations { get; set; } = new();
    public Collection<AccountLink> Participants { get; set; } = new();
    public HiscoreField Activity { get; set; }
    
    public bool IsActive { get; set; }
}

public enum EventType
{
    BossOfTheWeek, 
    SkillOfTheWeek
}