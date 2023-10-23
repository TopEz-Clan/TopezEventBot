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
public class BossOfTheWeekModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRunescapeHiscoreHttpClient _rsClient;

    public BossOfTheWeekModule(IServiceScopeFactory scopeFactory, IRunescapeHiscoreHttpClient rsClient)
    {
        _scopeFactory = scopeFactory;
        _rsClient = rsClient;
    }

    [SlashCommand("start-wildy", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildyBotw(WildyBosses boss, bool isActive = true)
    {
        await StartBotw((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("start-group", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildyBotw(GroupBosses boss, bool isActive = true)
    {
        await StartBotw((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("start-quest", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(QuestBosses boss, bool isActive = true)
    {
        await StartBotw((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("slayer", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(SlayerBosses boss, bool isActive = true)
    {
        await StartBotw((HiscoreField)boss, isActive);
    }
    
    [SlashCommand("start-world", "Start boss of the week")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartQuestBotw(WorldBosses boss, bool isActive = true)
    {
        await StartBotw((HiscoreField)boss, isActive);
    }
    
    private async Task StartBotw(HiscoreField boss, bool isActive = true)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var eventId = await ctx.CreateEvent(EventType.BossOfTheWeek, boss, isActive);
        var componentBuilder = new ComponentBuilder();
        var threadId = await NewThreadInCurrentChannelAsync(boss);
        componentBuilder.AddRow(new ActionRowBuilder().WithButton("Register",$"register_event:{eventId},{threadId}", ButtonStyle.Success));
        await RespondAsync(embed: EmbedGenerator.BossOfTheWeek(boss), components: isActive ? componentBuilder.Build() : null);
    }
    
    
    [ComponentInteraction("register_event:*,*", true)]
    public async Task RegisterForEvent(string eventIdAsString, string threadIdAsString) {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetService<TopezContext>();
        var eventId = long.Parse(eventIdAsString);
        var @event = db.Events.FirstOrDefault(e => e.Id == eventId);
        if (@event == null) {
            await RespondAsync("It seems this event has been deleted, please contact the Coordinator team", ephemeral: true);
            return;
        }
        if (!@event.IsActive) {
            await RespondAsync("The event has already concluded!", ephemeral: true);
            return;
        }
        var memberId = Context.User.Id;
        var linkedAccount = db.AccountLinks.FirstOrDefault(x => x.DiscordMemberId == memberId);
        if (linkedAccount == null) {
            await RespondAsync("You have no linked account yet - please link your runescape account first with the */link-rsn* command", ephemeral:true);
            return;
        }

        var player = await _rsClient.LoadPlayer(linkedAccount.RunescapeName);
        
        @event.EventParticipations.Add(new EventParticipation()
        {
            Event = @event,
            AccountLink = linkedAccount,
            StartingPoint = player.Bosses[@event.Activity].KillCount
        });
        
        await db.SaveChangesAsync();
        
        var threadId = ulong.Parse(threadIdAsString);
        await Context.Guild.ThreadChannels.FirstOrDefault(x => x.Id == threadId).SendMessageAsync($"{player.UserName} Registered for Boss {@event.Activity.GetDisplayName()} with *{player.Bosses[@event.Activity].KillCount} KC*");
        await RespondAsync($"Registered {player.UserName} for Boss {@event.Activity.GetDisplayName()} with *{player.Bosses[@event.Activity].KillCount} KC*", ephemeral: true);
    }
    
    [SlashCommand("finish", "Sets the last boss of the week command to inactive and compiles winner-data")]
    public async Task FinishEvent()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetService<TopezContext>();

        var lastActiveBotwEvent = db.Events.Where(x => x.Type == EventType.BossOfTheWeek)
            .Include(x => x.EventParticipations)
            .ThenInclude(p => p.AccountLink)
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();
        lastActiveBotwEvent.IsActive = false;
        db.Events.Update(lastActiveBotwEvent);
        
        var participants = lastActiveBotwEvent.EventParticipations;
        foreach (var p in participants)
        {
            var player = await _rsClient.LoadPlayer(p.AccountLink.RunescapeName);
            p.EndPoint = player.Bosses[lastActiveBotwEvent.Activity].KillCount;
        }
        db.UpdateRange(participants);
        await db.SaveChangesAsync();

        var result = participants.Select(p => new
        {
            Account = p.AccountLink.RunescapeName,
            Begin = p.StartingPoint,
            End = p.EndPoint,
            KillCountMade = p.EndPoint - p.StartingPoint
        }).OrderByDescending(x => x.KillCountMade);

        var first = result.FirstOrDefault();
        await ReplyAsync($"Winner of this weeks Boss of the Week is {first.Account} with {first.KillCountMade} KC! Congratulations!");
        var resultTable = result.ToArray().ToMarkdownTable();
        await RespondAsync($@"```{result.ToMarkdownTable()}```");
    }
    
    private async Task<ulong> NewThreadInCurrentChannelAsync(HiscoreField activity)
    {
        var channel = Context.Channel as ITextChannel;
        var newThread = await channel.CreateThreadAsync(
            name: $"Boss of the Week - {activity.GetDisplayName()}",
            autoArchiveDuration: ThreadArchiveDuration.OneWeek,
            invitable: false,
            type: ThreadType.PublicThread
        );
        return newThread.Id;
    }
}