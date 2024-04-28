using Coravel.Invocable;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;
using TopezEventBot.Util;

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
            ScottPlot.Plot plot = new();
            plot.HideLegend();

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

            var ticks = new Tick[eventProgress.Count()];
            plot.Add.Palette = new ScottPlot.Palettes.Nord();

            foreach (var tuple in eventProgress.OrderByDescending(p => p.Progress).Select((participation, index) => (participation, index)))
            {
                plot.Add.Bar(position: tuple.index + 1, value: tuple.participation.Progress);
                ticks[tuple.index] = new Tick(tuple.index + 1, tuple.participation.RunescapeName);
            }

            plot.Axes.Left.Label.Text = activeEvent.Type switch
            {
                TrackableEventType.BossOfTheWeek => "Kills",
                TrackableEventType.SkillOfTheWeek => "Experience",
                _ => throw new ArgumentOutOfRangeException(nameof(activeEvent.Type), activeEvent.Type, null)
            };

            // plot.FigureBackground.Color = Color.FromHex("#181818");
            // plot.DataBackground.Color = Color.FromHex("#1f1f1f");
            plot.DataBackground.Color = Colors.Black.WithAlpha(.5);
            plot.FigureBackground.Image = new(Convert.FromBase64String(Constants.image));

            // change axis and grid colors
            plot.Axes.Color(Color.FromHex("#d7d7d7"));
            plot.Grid.MajorLineColor = Color.FromHex("#404040");

            // change legend colors
            plot.Legend.BackgroundColor = Color.FromHex("#404040");
            plot.Legend.FontColor = Color.FromHex("#d7d7d7");
            plot.Legend.OutlineColor = Color.FromHex("#d7d7d7");

            plot.Axes.Left.Label.ForeColor = Colors.Magenta;
            plot.Axes.Left.Label.Italic = true;

            plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            plot.Axes.Bottom.MajorTickStyle.Length = 0;
            plot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            plot.Axes.Margins(bottom: 0);

            // display the legend in a LegendPanel outside the plot
            _logger.LogCritical("saving demo.png");
            var tempFile = Path.GetTempFileName() + Guid.NewGuid() + ".png";
            plot.SavePng(tempFile, 400, 300);
        }

        await _db.SaveChangesAsync();
    }
}
