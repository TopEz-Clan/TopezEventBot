using Discord;
using Discord.Interactions;
using TopezEventBot.Data.Entities;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.Mass;

[Group("mass", "Command group for mass events")]
public class MassEventModule : SchedulableEventModuleBase
{
    public MassEventModule(IServiceScopeFactory scopeFactory) : base(scopeFactory, SchedulableEventType.Mass)
    {
    }
    
    [SlashCommand("schedule", "Schedule a new mass event")]
    [RequireRole("Coordinator")]
    public async Task ScheduleEvent(MassEventActivityChoice activity, string location, DateTime time)
    {
        await base.ScheduleEvent((HiscoreField)activity, location, time);
    }

    protected override Embed GetEmbed(HiscoreField activity, string location, DateTime time)
    {
        return EmbedGenerator.Mass(activity, location, time);
    }

    [ComponentInteraction("list-participants-mass:*", ignoreGroupNames: true)]
    public override async Task ListParticipants(long eventId)
    {
        await ListEventParticipants(eventId);
    }
    
    [ComponentInteraction("register-for-mass:*", ignoreGroupNames: true)]
    public override async Task HandleRegistration(long eventId)
    {
        await HandleEventRegistration(eventId);
    }
}