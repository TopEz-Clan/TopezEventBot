using TopezEventBot.Data.Entities;

namespace TopezEventBot.Data.Models.Extensions;

public static class TrackableEventTypeExtensions
{
    public static string GetDisplayName(this TrackableEventType type) => type switch
    {
        TrackableEventType.BossOfTheWeek => "Boss of the Week",
        TrackableEventType.SkillOfTheWeek => "Skill of the Week",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static string Unit(this TrackableEventType type) => type switch
    {
        TrackableEventType.BossOfTheWeek => "kc",
        TrackableEventType.SkillOfTheWeek => "xp",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static string EventButtonTag(this TrackableEventType type, long eventId, ulong threadId) => $"register-for-{type switch { TrackableEventType.BossOfTheWeek => "botw", TrackableEventType.SkillOfTheWeek => "sotw", _ => throw new ArgumentOutOfRangeException(nameof(type), type, null) }}:{eventId},{threadId}";
}