using ScottPlot;
using TopezEventBot.Data.Entities;
using TopezEventBot.Invocables;

namespace TopezEventBot.Util;

public class BarPlotBuilder : IBarPlotBuilder
{
    private ScottPlot.Plot _plot;

    public BarPlotBuilder()
    {
        _plot = new Plot();
        SetDefaults();
    }

    private void SetDefaults()
    {
        _plot.Add.Palette = new ScottPlot.Palettes.Nord();
        // _plot.FigureBackground.Color = Color.FromHex("#181818");
        // _plot.DataBackground.Color = Color.FromHex("#1f1f1f");
        _plot.DataBackground.Color = Colors.Black.WithAlpha(.5);

        // change axis and grid colors
        _plot.Axes.Color(Color.FromHex("#d7d7d7"));
        _plot.Grid.MajorLineColor = Color.FromHex("#404040");

        // change legend colors
        _plot.Legend.BackgroundColor = Color.FromHex("#404040");
        _plot.Legend.FontColor = Color.FromHex("#d7d7d7");
        _plot.Legend.OutlineColor = Color.FromHex("#d7d7d7");

        _plot.Axes.Left.Label.ForeColor = Colors.Magenta;
        _plot.Axes.Left.Label.Italic = true;

        _plot.FigureBackground.Image = new(Convert.FromBase64String(Constants.image));
        _plot.HideLegend();

        // tell the plot to autoscale with no padding beneath the bars
        _plot.Axes.Margins(bottom: 0);
    }

    public IBarPlotBuilder WithBars(IEnumerable<ParticipantProgress> data)
    {
        var ticks = new Tick[data.Count()];

        foreach (var tuple in data.OrderByDescending(p => p.Progress).Select((participation, index) => (participation, index)))
        {
            _plot.Add.Bar(position: tuple.index + 1, value: tuple.participation.Progress);
            ticks[tuple.index] = new Tick(tuple.index + 1, tuple.participation.RunescapeName);
        }

        _plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
        _plot.Axes.Bottom.MajorTickStyle.Length = 0;
        _plot.HideGrid();
        return this;
    }

    public IBarPlotBuilder ForEvent(TrackableEvent trackableEvent)
    {
        var text = (bool unit) => trackableEvent.Type switch
        {
            TrackableEventType.BossOfTheWeek => unit ? "Kills" : $"Boss of the Week - {trackableEvent.Activity}",
            TrackableEventType.SkillOfTheWeek => unit ? "Experience" : $"Skill of the Week - {trackableEvent.Activity}",
            _ => throw new ArgumentOutOfRangeException(nameof(trackableEvent.Type), trackableEvent.Type, null)
        };

        _plot.Axes.Left.Label.Text = text(true);
        _plot.Title(text(false));

        return this;
    }

    public IBarPlotBuilder WithImage(Image image)
    {
        _plot.FigureBackground.Image = image;
        return this;
    }

    public ScottPlot.Plot Build()
    {
        return _plot;
    }
}

