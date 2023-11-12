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
    private readonly int _experience;
    private readonly int _level;
    private readonly int _rank;

    [Index(0)]
    public int Rank
    {
        get => _rank;
        init => _rank = value == -1 ? 0 : value;
    }

    [Index(1)]
    public int Level
    {
        get => _level;
        init => _level = value == -1 ? 0 : value;
    }

    [Index(2)]
    public int Experience
    {
        get => _experience;
        init => _experience = value == -1 ? 0 : value;
    }
}

public class ActivityStats
{
    private readonly int _killCount;
    private readonly int _rank;

    [Index(0)]
    public int Rank
    {
        get => _rank;
        init => _rank = ( value == -1) ? 0 : value;
    }

    [Index(1)]
    public int KillCount
    {
        get => _killCount;
        init => _killCount = (value == -1) ? 0 : value;
    }
}