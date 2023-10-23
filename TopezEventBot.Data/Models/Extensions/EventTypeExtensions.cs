using TopezEventBot.Data.Entities;

namespace TopezEventBot.Util.Extensions;

public static class EventTypeExtensions
{
    public static string GetDisplayName(this EventType type) => type switch
    {
        EventType.BossOfTheWeek => "Boss of the Week",
        EventType.SkillOfTheWeek => "Skill of the Week",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static string Unit(this EventType type) => type switch
    {
        EventType.BossOfTheWeek => "kc",
        EventType.SkillOfTheWeek => "xp",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static string EventButtonTag(this EventType type, long eventId, ulong threadId) => $"register-for-{type switch { EventType.BossOfTheWeek => "botw", EventType.SkillOfTheWeek => "sotw", _ => throw new ArgumentOutOfRangeException(nameof(type), type, null) }}:{eventId},{threadId}";
}