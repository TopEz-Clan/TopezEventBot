using CsvHelper.Configuration.Attributes;
using TopezEventBot.Util;

namespace TopezEventBot.Http.Models;

public class Player
{
    public string UserName { get; set; }

    public Dictionary<HiscoreField, SkillStats> Skills { get; set; } = new();
    public Dictionary<HiscoreField, ActivityStats> Activities { get; set; } = new();
    public Dictionary<HiscoreField, ActivityStats> Bosses { get; set; } = new();
}

public class SkillStats
{

    [Index(0)]
    public int Rank { get; init; }
    [Index(1)]
    public int Level { get; init; }
    [Index(2)]
    public int Experience { get; init; }

}

public class ActivityStats
{

    [Index(0)]
    public int Rank { get; init; }
    [Index(1)]
    public int KillCount { get; init; }
}