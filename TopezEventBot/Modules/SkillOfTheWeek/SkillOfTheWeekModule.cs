using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.SkillOfTheWeek;

[Group("sotw", "All skill of the week related commands")]
public class SkillOfTheWeekModule : TrackableEventModuleBase
{
    public SkillOfTheWeekModule(IDbContextFactory<TopezContext> contextFactory, IRunescapeHiscoreHttpClient rsClient) : base(
        contextFactory, rsClient, TrackableEventType.SkillOfTheWeek)
    {
    }

    [SlashCommand("start", "Start skill of the week!")]
    [RequireRole("Coordinator")]
    public async Task StartSkillOfTheWeek(SkillOfTheWeekChoice sotw, bool isActive = true)
    {
        await StartEvent((HiscoreField)sotw, isActive);
    }

    [ComponentInteraction("register-for-sotw:*,*", ignoreGroupNames: true)]
    public async Task RegisterForSotw(long eventId, ulong threadId)
    {
        await RegisterForEvent(eventId, threadId);
    }

    [ComponentInteraction("list-participants-sotw:*", ignoreGroupNames: true)]
    public override async Task ListParticipants(long eventId)
    {
        await ListEventParticipants(eventId);
    }
}