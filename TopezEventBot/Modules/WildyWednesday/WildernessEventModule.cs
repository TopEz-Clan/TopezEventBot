using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.WildyWednesday;

[Group("wildy", "Commands for wildy-related events")]
public class WildernessEventModule : SchedulableEventModuleBase
{
    [SlashCommand("schedule", "Schedule a new wildy event")]
    [RequireRole("Coordinator")]
    public async Task StartWildyWednesday(WildyWednesdayActivityChoice activity, string location, DateTime time)
    {
        await ScheduleEvent((HiscoreField)activity, location, time);
    }

    public WildernessEventModule(IDbContextFactory<TopezContext> contextFactory) : base(contextFactory, SchedulableEventType.WildyWednesday)
    {
    }

    [ComponentInteraction("register-for-ww:*", ignoreGroupNames: true)]
    public override async Task HandleRegistration(long eventId)
    {
        await HandleEventRegistration(eventId);
    }

    
    [ComponentInteraction("list-participants-ww:*", ignoreGroupNames: true)]
    public override async Task ListParticipants(long eventId)
    {
        await ListEventParticipants(eventId);
    }

    protected override Embed GetEmbed(HiscoreField activity, string location, DateTime time)
    {
        return EmbedGenerator.WildyWednesday(activity, location, time);
    }
}