using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Extensions;
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
    public async Task StartWildyBotw(WildyBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("group", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildyBotw(GroupBosses boss, bool isActive = true)
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
    public async Task StartQuestBotw(SlayerBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("world", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(WorldBosses boss, bool isActive = true)
    {
        await StartEvent((HiscoreField)boss, isActive);
    }

    [ComponentInteraction("register-for-botw:*,*", ignoreGroupNames: true)]
    public async Task RegisterForBotw(string eventId, string threadId)
    {
        await RegisterForEvent(eventId, threadId);
    }

}