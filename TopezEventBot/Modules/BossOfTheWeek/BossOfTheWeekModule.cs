using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;
using TopezEventBot.Modules.SkillOfTheWeek;
using TopezEventBot.Util;
using TopezEventBot.Util.Extensions;

namespace TopezEventBot.Modules.BossOfTheWeek;

[Group("botw", "Start boss of the week")]
public class BossOfTheWeekModule : TrackableEventModuleBase
{
    public BossOfTheWeekModule(IServiceScopeFactory scopeFactory, IRunescapeHiscoreHttpClient rsClient) : base(scopeFactory, rsClient, TrackableEventType.BossOfTheWeek)
    {
    }

    [SlashCommand("wildy", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildernessBotw(WildyBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("group", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartGroupBotw(GroupBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("quest", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(QuestBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("slayer", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartSlayerBotw(SlayerBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("world", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWorldBotw(WorldBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }

    [ComponentInteraction("register-for-botw:*,*", ignoreGroupNames: true)]
    public async Task RegisterForBotw(long eventId, ulong threadId)
    {
        await RegisterForEvent(eventId, threadId);
    }
    
    [ComponentInteraction("list-participants-botw:*", ignoreGroupNames: true)]
    public override async Task ListParticipants(long eventId)
    {
        await ListEventParticipants(eventId);
    }
}