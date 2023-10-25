using TopezEventBot.Data.Entities;

namespace TopezEventBot.Data.Models.Extensions;

public static class SchedulableEventTypeExtensions
{
    public static string GetShortIdentifier(this SchedulableEventType type) => type switch
    {
        SchedulableEventType.WildyWednesday => "ww",
        SchedulableEventType.Mass => "mass",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}