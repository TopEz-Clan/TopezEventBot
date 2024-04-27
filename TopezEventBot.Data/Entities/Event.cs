using System.Collections.ObjectModel;
using TopezEventBot.Util;

namespace TopezEventBot.Data.Entities;

public abstract class Event<TType> where TType : Enum
{
    public long Id { get; set; }
    public TType Type { get; set; }
    public Collection<AccountLink> Participants { get; set; } = new();
    public HiscoreField Activity { get; set; }
}
