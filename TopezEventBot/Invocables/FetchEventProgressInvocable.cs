using Coravel.Invocable;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;
using TopezEventBot.Util;

namespace TopezEventBot.Invocables;

public record ParticipantProgress(string RunescapeName, int Progress);

public class FetchEventProgressInvocable : IInvocable
{
    private readonly TopezContext _db;
    private readonly DiscordSocketClient _client;
    private readonly IRunescapeHiscoreHttpClient _hiscoreClient;
    private readonly ILogger<FetchEventProgressInvocable> _logger;

    public FetchEventProgressInvocable(TopezContext db, DiscordSocketClient client, IRunescapeHiscoreHttpClient hiscoreClient, ILogger<FetchEventProgressInvocable> logger)
    {
        _db = db;
        _client = client;
        _hiscoreClient = hiscoreClient;
        _logger = logger;
    }

    public async Task Invoke()
    {
        var activeEvents = _db.TrackableEvents
            .AsSplitQuery()
            .Include(x => x.EventParticipations)
            .ThenInclude(x => x.AccountLink)
            .Include(x => x.EventParticipations)
            .ThenInclude(x => x.Progress)
            .Where(x => x.IsActive);

        _logger.LogCritical("Start fetching for active event");
        var fetchTime = DateTime.Now;
        foreach (var activeEvent in activeEvents)
        {
            var eventProgress = new List<ParticipantProgress>();

            foreach (var participant in activeEvent.EventParticipations)
            {
                var hiscoreData = await _hiscoreClient.LoadPlayer(participant.AccountLink.RunescapeName);
                participant.Progress.Add(new TrackableEventProgress()
                {
                    FetchedAt = fetchTime,
                    Progress = activeEvent.Type switch
                    {
                        TrackableEventType.BossOfTheWeek => hiscoreData.Bosses[activeEvent.Activity].KillCount,
                        TrackableEventType.SkillOfTheWeek => hiscoreData.Skills[activeEvent.Activity].Experience,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                });
                eventProgress.Add(new(participant.AccountLink.RunescapeName, participant.Progress.Last().Progress - participant.StartingPoint));
            }
            _db.Update(activeEvent);

            var tempFile = Path.GetTempFileName() + Guid.NewGuid() + ".png";

            var plot = new BarPlotBuilder()
                .WithBars(eventProgress)
                // .WithImage(new(Convert.FromBase64String(Constants.image)))
                .ForEvent(activeEvent)
                .Build()
                .SavePng(tempFile, 1600, 1200);

            _logger.LogCritical("saving demo.png");
        }

        await _db.SaveChangesAsync();
    }
}
