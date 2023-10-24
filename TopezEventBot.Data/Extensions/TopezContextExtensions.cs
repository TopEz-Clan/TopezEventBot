using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Util;

namespace TopezEventBot.Extensions;

public static class TopezContextExtensions
{
    public static async Task<long> CreateEvent(this TopezContext ctx, TrackableEventType type, HiscoreField activity, bool isActive) {
        var @event = new TrackableEvent()
        {
            Type = type,
            Activity = activity,
            IsActive = isActive
        };
        var e = await ctx.TrackableEvents.AddAsync(@event);
        await ctx.SaveChangesAsync();
        return e.Entity.Id;
    }
}