using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Data.Extensions;
using TopezEventBot.Data.Models.Extensions;
using TopezEventBot.Http;
using TopezEventBot.Util;
using TopezEventBot.Util.Extensions;

namespace TopezEventBot.Modules;

public abstract class TrackableEventModuleBase : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IDbContextFactory<TopezContext> _contextFactory;
    private readonly IRunescapeHiscoreHttpClient _rsClient;
    private readonly TrackableEventType _eventType;

    protected TrackableEventModuleBase(IDbContextFactory<TopezContext> contextFactory, IRunescapeHiscoreHttpClient rsClient,
            TrackableEventType eventType)
    {
        _contextFactory = contextFactory;
        _rsClient = rsClient;
        _eventType = eventType;
    }

    protected async Task RegisterForEvent(long eventId, ulong threadId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        await DeferAsync(ephemeral: true);
        var @event = db.TrackableEvents.FirstOrDefault(e => e.Id == eventId);
        var memberId = Context.User.Id;
        var linkedAccount = db.AccountLinks.FirstOrDefault(x => x.DiscordMemberId == memberId);
        if (@event == null)
        {
            await FollowupAsync("It seems this event has been deleted, please contact the Coordinator team",
                    ephemeral: true);
            return;
        }

        if (!@event.IsActive)
        {
            await FollowupAsync("The event has already concluded!", ephemeral: true);
            return;
        }

        if (linkedAccount == null)
        {
            await FollowupAsync(
                    "You have no linked account yet - please link your runescape account first with the */link-rsn* command",
                    ephemeral: true);
            return;
        }

        var player = await _rsClient.LoadPlayer(linkedAccount.RunescapeName);

        try
        {
            @event.EventParticipations.Add(new TrackableEventParticipation()
            {
                Event = @event,
                AccountLink = linkedAccount,
                StartingPoint = _eventType switch
                {
                    TrackableEventType.BossOfTheWeek => player.Bosses[@event.Activity].KillCount,
                    TrackableEventType.SkillOfTheWeek => player.Skills[@event.Activity].Experience,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });

            await db.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            await FollowupAsync($"You already participate in this {_eventType.GetDisplayName()}", ephemeral: true);
            return;
        }

        var eventType = _eventType switch
        {
            TrackableEventType.BossOfTheWeek => "Boss",
            TrackableEventType.SkillOfTheWeek => "Skill",
            _ => throw new ArgumentOutOfRangeException(nameof(_eventType), _eventType, null)
        };

        var startProgress = _eventType switch
        {
            TrackableEventType.BossOfTheWeek => player.Bosses[@event.Activity].KillCount,
            TrackableEventType.SkillOfTheWeek => player.Skills[@event.Activity].Experience,
            _ => throw new ArgumentOutOfRangeException()
        };

        var message =
            $"Registered {player.UserName} for {eventType} {@event.Activity.GetDisplayName()} with *{startProgress} {_eventType.Unit()}*";
        await Context.Guild.ThreadChannels.FirstOrDefault(x => x.Id == threadId)?.SendMessageAsync(message)!;
        await FollowupAsync(message, ephemeral: true);
    }

    [SlashCommand("finish", "Finish event")]
    [RequireRole("Coordinator")]
    public async Task FinishEvent()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();

        await DeferAsync();
        var lastActiveEvent = db.TrackableEvents.Where(x => x.Type == _eventType)
            .Include(x => x.EventParticipations)
            .ThenInclude(p => p.AccountLink)
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();

        if (lastActiveEvent == null)
        {
            await FollowupAsync($"There's no active {_eventType.GetDisplayName()} event ongoing!", ephemeral: true);
            return;
        }

        lastActiveEvent.IsActive = false;
        db.TrackableEvents.Update(lastActiveEvent);

        var participants = lastActiveEvent.EventParticipations;
        if (!participants.Any())
        {
            await FollowupAsync("Sadly there were no participants this time! :(");
            return;
        }

        await Parallel.ForEachAsync(participants, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, async (participation, token) =>
                {
                    var player = await _rsClient.LoadPlayer(participation.AccountLink.RunescapeName);
                    participation.EndPoint = _eventType switch
                    {
                        TrackableEventType.BossOfTheWeek => player.Bosses[lastActiveEvent.Activity].KillCount,
                        TrackableEventType.SkillOfTheWeek => player.Skills[lastActiveEvent.Activity].Experience,
                        _ => throw new ArgumentOutOfRangeException(nameof(_eventType), _eventType, null)
                    };
                });

        db.UpdateRange(participants);
        await db.SaveChangesAsync();

        var result = participants.Select(p =>
                new EventResult(p.AccountLink.RunescapeName, p.StartingPoint, p.EndPoint, p.EndPoint - p.StartingPoint,
                    p.AccountLink.DiscordMemberId)
                ).OrderByDescending(x => x.Progress).Take(3);

        var firstPlace = result.FirstOrDefault();
        await ReplyAsync(
                $"Winner of this weeks {_eventType.GetDisplayName()} is {MentionUtils.MentionUser(firstPlace.DiscordUserDisplayName)} with {firstPlace.Progress} {_eventType.Unit()}! Congratulations!",
                allowedMentions: AllowedMentions.All);
        await FollowupAsync(embed: EmbedGenerator.EventWinner(_eventType, lastActiveEvent.Activity, result));
    }

    protected async Task StartEvent(HiscoreField activity, bool isActive = true)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var eventId = await db.CreateEvent(_eventType, activity, isActive);
        var componentBuilder = new ComponentBuilder();
        var threadId = await NewThreadInCurrentChannelAsync(activity, _eventType);
        var eventAbbrev = _eventType switch
        {
            TrackableEventType.BossOfTheWeek => "botw",
            TrackableEventType.SkillOfTheWeek => "sotw",
            _ => throw new ArgumentOutOfRangeException()
        };

        componentBuilder.AddRow(new ActionRowBuilder()
                .WithButton("Register", $"register-for-{eventAbbrev}:{eventId},{threadId}", ButtonStyle.Success)
                .WithButton("List participants", $"list-participants-{eventAbbrev}:{eventId}", ButtonStyle.Secondary));
        await DeferAsync();
        await FollowupAsync(embed: _eventType switch
        {
            TrackableEventType.BossOfTheWeek => EmbedGenerator.BossOfTheWeek(activity),
            TrackableEventType.SkillOfTheWeek => EmbedGenerator.SkillOfTheWeek(activity),
            _ => throw new ArgumentOutOfRangeException()
        }, components: isActive ? componentBuilder.Build() : null);
    }

    public abstract Task ListParticipants(long eventId);

    private async Task<ulong> NewThreadInCurrentChannelAsync(HiscoreField activity, TrackableEventType type)
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

    protected async Task ListEventParticipants(long eventId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        await DeferAsync(ephemeral: true);

        var @event = db.TrackableEvents.Include(x => x.Participants).FirstOrDefault(x => x.Id == eventId);

        if (@event == null)
        {
            await FollowupAsync("The event seems to be deleted", ephemeral: true);
            return;
        }

        if (!@event.Participants.Any())
        {
            await FollowupAsync("There's no participants for this event yet :(", ephemeral: true);
            return;
        }

        if (!@event.Participants.Select(x => x.DiscordMemberId).Contains(Context.User.Id))
        {
            await FollowupAsync("You need to be registered to see the participants!", ephemeral: true);
            return;
        }

        var msg = @event.Participants.Aggregate("The following people are participating: \n\n",
                (current, accountLink) => current + "* " + (accountLink.RunescapeName + "\n"));

        await FollowupAsync(msg, ephemeral: true);
    }

    [SlashCommand("leaderboard", "show leaderboard, either by wins or total xp/kc")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task Leaderboard()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();

        await DeferAsync(ephemeral: true);
        var playerScores = new Dictionary<string, int>();
        var events = db.TrackableEvents.Where(x => x.Type == _eventType).Include(x => x.EventParticipations)
            .ThenInclude(x => x.AccountLink);
        foreach (var @event in events.Where(x => x.EventParticipations.Any()))
        {
            var winner = @event.EventParticipations.ToList().Select(x => new
            { x.AccountLink.RunescapeName, Progress = x.EndPoint - x.StartingPoint }).MaxBy(x => x.Progress);
            if (winner == null) continue;
            playerScores.TryAdd(winner.RunescapeName, 0);
            playerScores[winner.RunescapeName]++;
        }

        if (!playerScores.Any())
        {
            await FollowupAsync("Not enough data for an official leaderboard yet!", ephemeral: true);
            return;
        }

        var i = 0;
        var resultMsg = $"All time leaderboard for {_eventType.GetDisplayName()}\n\n```";
        resultMsg = playerScores.Select(x => new { Player = x.Key, Wins = x.Value }).OrderByDescending(x => x.Wins)
            .Aggregate(resultMsg,
                    (current, playerWins) =>
                    current +
                    $"{++i}. {playerWins.Player} - {playerWins.Wins} win{(playerWins.Wins == 1 ? string.Empty : 's')}\n");
        resultMsg += "```";

        await FollowupAsync(resultMsg, ephemeral: true);
    }
}
