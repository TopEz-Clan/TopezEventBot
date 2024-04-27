using Coravel.Invocable;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;

namespace TopezEventBot.Invocables;

record ParticipantProgress(string RunescapeName, int Progress);

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
            ScottPlot.Plot myPlot = new();
            myPlot.HideLegend();

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

            var result = activeEvent.EventParticipations.Select((p, i) => new
            {
                Index = i,
                p.AccountLink.RunescapeName,
                Progress = p.EndPoint - p.StartingPoint
            }).OrderByDescending(p => p.Progress);

            var ticks = new Tick[result.Count()];
            var idx = 0;
            foreach (var participation in result)
            {
                myPlot.Add.Bar(position: participation.Index + 1, value: participation.Progress);
                ticks[idx++] = new Tick(participation.Index + 1, participation.RunescapeName);
            }

            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            myPlot.Axes.Margins(bottom: 0);

            // display the legend in a LegendPanel outside the plot
            _logger.LogCritical("saving demo.png");
            myPlot.SavePng("demo.png", 400, 300);
        }

        await _db.SaveChangesAsync();
    }
}
