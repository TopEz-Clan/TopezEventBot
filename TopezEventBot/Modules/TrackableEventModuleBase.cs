using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Extensions;
using TopezEventBot.Http;
using TopezEventBot.Http.Models;
using TopezEventBot.Util;
using TopezEventBot.Util.Extensions;

namespace TopezEventBot.Modules;

public abstract class TrackableEventModuleBase : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRunescapeHiscoreHttpClient _rsClient;
    private readonly EventType _type;

    protected TrackableEventModuleBase(IServiceScopeFactory scopeFactory, IRunescapeHiscoreHttpClient rsClient, EventType type)
    {
        _scopeFactory = scopeFactory;
        _rsClient = rsClient;
        _type = type;
    }

    protected async Task RegisterForEvent(string eventIdAsString, string threadIdAsString) {
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

        try
        {
            @event.EventParticipations.Add(new EventParticipation()
            {
                Event = @event,
                AccountLink = linkedAccount,
                StartingPoint = _type switch
                {
                    EventType.BossOfTheWeek => player.Bosses[@event.Activity].KillCount,
                    EventType.SkillOfTheWeek => player.Skills[@event.Activity].Experience,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });
        
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            await RespondAsync($"You already participate in this {_type.GetDisplayName()}", ephemeral:true);
            return;
        }
        
        var threadId = ulong.Parse(threadIdAsString);
        var eventType = _type switch { EventType.BossOfTheWeek => "Boss", EventType.SkillOfTheWeek => "Skill", _ => throw new ArgumentOutOfRangeException(nameof(_type), _type, null) };
        var startProgress = _type switch {
            EventType.BossOfTheWeek => $"{player.Bosses[@event.Activity].KillCount} KC",
            EventType.SkillOfTheWeek => $"{player.Skills[@event.Activity].Experience} XP",
            _ => throw new ArgumentOutOfRangeException(nameof(_type), _type, "Unknown activity type") };
        var message = $"Registered {player.UserName} for {eventType} {@event.Activity.GetDisplayName()} with *{startProgress}*";
        await Context.Guild.ThreadChannels.FirstOrDefault(x => x.Id == threadId)?.SendMessageAsync(message)!;
        await RespondAsync( message, ephemeral: true);
    }

    [SlashCommand("finish", "Sets the last boss of the week command to inactive and compiles winner-data")]
    public async Task FinishEvent()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetService<TopezContext>();

        var lastActiveEvent = db.Events.Where(x => x.Type == _type)
            .Include(x => x.EventParticipations)
            .ThenInclude(p => p.AccountLink)
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();
        
        lastActiveEvent.IsActive = false;
        db.Events.Update(lastActiveEvent);
        
        var participants = lastActiveEvent.EventParticipations;
        if (!participants.Any()) {
            await RespondAsync("Sadly there were no participants this time! :(");
            return;
        }
        
        foreach (var p in participants)
        {
            var player = await _rsClient.LoadPlayer(p.AccountLink.RunescapeName);
            p.EndPoint = _type switch
            {
                EventType.BossOfTheWeek => player.Bosses[lastActiveEvent.Activity].KillCount,
                EventType.SkillOfTheWeek => player.Skills[lastActiveEvent.Activity].Experience,
                _ => throw new ArgumentOutOfRangeException(nameof(_type), _type, null)
            };
        }
        db.UpdateRange(participants);
        await db.SaveChangesAsync();
        

        var result = participants.Select(p =>
            new EventResult(p.AccountLink.RunescapeName, p.StartingPoint, p.EndPoint, p.EndPoint - p.StartingPoint, p.AccountLink.DiscordMemberId)
        ).Take(3).OrderByDescending(x => x.Progress);

        var firstPlace = result.FirstOrDefault();
        await ReplyAsync($"Winner of this weeks {_type.GetDisplayName()} is {MentionUtils.MentionUser(firstPlace.DiscordUserDisplayName)} with {firstPlace.Progress} {_type.Unit()}! Congratulations!", allowedMentions: AllowedMentions.All);
        await RespondAsync(embed: EmbedGenerator.EventWinner(_type, lastActiveEvent.Activity, result));
    }

    protected async Task StartEvent(HiscoreField activity, bool isActive = true)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var eventId = await ctx.CreateEvent(_type, activity, isActive);
        var componentBuilder = new ComponentBuilder();
        var threadId = await NewThreadInCurrentChannelAsync(activity, _type);
        var eventAbbrev = _type switch
        {
            EventType.BossOfTheWeek => "botw",
            EventType.SkillOfTheWeek => "sotw",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        componentBuilder.AddRow(new ActionRowBuilder().WithButton("Register",$"register-for-{eventAbbrev}:{eventId},{threadId}", ButtonStyle.Success));
        await RespondAsync(embed: _type switch {
            EventType.BossOfTheWeek => EmbedGenerator.BossOfTheWeek(activity),
            EventType.SkillOfTheWeek => EmbedGenerator.SkillOfTheWeek(activity),
            _ => throw new ArgumentOutOfRangeException()
        }, components: isActive ? componentBuilder.Build() : null);
    }

    private async Task<ulong> NewThreadInCurrentChannelAsync(HiscoreField activity, EventType type)
    {
        var channel = Context.Channel as ITextChannel;
        var newThread = await channel.CreateThreadAsync(
            
            name: $"{type.GetDisplayName()} - {activity.GetDisplayName()}",
            autoArchiveDuration: ThreadArchiveDuration.OneWeek,
            invitable: false,
            type: ThreadType.PublicThread
        );
        return newThread.Id;
    }


    [SlashCommand("leaderboard", "show leaderboard, either by wins or total xp/kc")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task Leaderboard()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetService<TopezContext>();
        
        var playerScores = new Dictionary<string, int>();
        var events = db.Events.Where(x => x.Type == _type).Include(x => x.EventParticipations).ThenInclude(x => x.AccountLink);
        foreach (var @event in events.Where(x => x.EventParticipations.Any()))
        {
            var winner = @event.EventParticipations.ToList().Select(x => new
                { x.AccountLink.RunescapeName, Progress = x.EndPoint - x.StartingPoint }).MaxBy(x => x.Progress);
            
            if (!playerScores.ContainsKey(winner.RunescapeName)) playerScores.Add(winner.RunescapeName, 0);
            playerScores[winner.RunescapeName]++;
        }
        
        if (!playerScores.Any())
        {
            await RespondAsync("Not enough data for an official leaderboard yet!", ephemeral: true);
            return;
        }

        var i = 0;
        var resultMsg = $"All time leaderboard for {_type.GetDisplayName()}\n\n```";
        resultMsg = playerScores.Select(x => new { Player = x.Key, Wins = x.Value }).OrderByDescending(x => x.Wins)
            .Aggregate(resultMsg,
                (current, playerWins) =>
                    current +
                    $"{++i}. {playerWins.Player} - {playerWins.Wins} win{(playerWins.Wins == 1 ? string.Empty : 's')}\n");
        resultMsg += "```";
        
        await RespondAsync(resultMsg, ephemeral: true);
    }
}