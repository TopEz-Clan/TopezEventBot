using ScottPlot;
using TopezEventBot.Data.Entities;
using TopezEventBot.Invocables;

namespace TopezEventBot.Util;

public interface IBarPlotBuilder
{
    IBarPlotBuilder ForEvent(TrackableEvent trackableEvent);
    IBarPlotBuilder WithImage(Image image);
    IBarPlotBuilder WithBars(IEnumerable<ParticipantProgress> data);
    ScottPlot.Plot Build();
}
