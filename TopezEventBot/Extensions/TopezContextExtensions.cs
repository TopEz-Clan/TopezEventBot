using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Util;

namespace TopezEventBot.Extensions;

public static class TopezContextExtensions
{
    public static async Task<long> CreateEvent(this TopezContext ctx, EventType type, HiscoreField activity, bool isActive) {
        var @event = new Event
        {
            Type = type,
            Activity = activity,
            IsActive = isActive
        };
        var e = await ctx.Events.AddAsync(@event);
        await ctx.SaveChangesAsync();
        return e.Entity.Id;
    }
}