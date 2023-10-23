using Discord;
using Discord.Interactions;
using TopezEventBot.Modules.SkillOfTheWeek;
using TopezEventBot.Util;
using TopezEventBot.Util.Extensions;

namespace TopezEventBot.Modules.BossOfTheWeek;

[Group("start-botw", "Start boss of the week")]
public class BossOfTheWeekModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("wildy", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildyBotw(WildyBosses boss, string codePhrase)
    {
        await StartBotw((HiscoreField)boss, codePhrase);
    }
    
    [SlashCommand("group", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildyBotw(GroupBosses boss, string codePhrase)
    {
        await StartBotw((HiscoreField)boss, codePhrase);
    }
    
    [SlashCommand("quest", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(QuestBosses boss, string codePhrase)
    {
        await StartBotw((HiscoreField)boss, codePhrase);
    }
    
    [SlashCommand("slayer", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(SlayerBosses boss, string codePhrase)
    {
        await StartBotw((HiscoreField)boss, codePhrase);
    }
    
    [SlashCommand("world", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(WorldBosses boss, string codePhrase)
    {
        await StartBotw((HiscoreField)boss, codePhrase);
    }
    
    private async Task StartBotw(HiscoreField boss, string codePhrase) {
        await RespondAsync(embed: EmbedGenerator.BossOfTheWeek(boss, codePhrase));
        await NewThreadInCurrentChannelAsync(boss);
    }
    
    private async Task NewThreadInCurrentChannelAsync(HiscoreField activity)
    {
        var channel = Context.Channel as ITextChannel;
        var newThread = await channel.CreateThreadAsync(
            name: $"Boss of the Week - {activity.GetDisplayName()}",
            autoArchiveDuration: ThreadArchiveDuration.OneWeek,
            invitable: false,
            type: ThreadType.PublicThread
        );
        await newThread.SendMessageAsync("Test");
    }
}