using Discord.Interactions;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Util;

namespace TopezEventBot.Modules;

public abstract class SchedulableEvent : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SchedulableEventType _type;

    protected SchedulableEvent(IServiceScopeFactory scopeFactory, SchedulableEventType type)
    {
        _scopeFactory = scopeFactory;
        _type = type;
    }

    protected async Task ScheduleEvent(HiscoreField activity, DateTime time)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();

        await db.SchedulableEvents.AddAsync(new Data.Entities.SchedulableEvent()
        {
            
        });




    }
    
    
    
    
}