﻿using System.Collections.ObjectModel;

namespace TopezEventBot.Data.Entities;

public class TrackableEventParticipation
{
    public long EventId { get; set; }
    public TrackableEvent Event { get; set; }
    public long AccountLinkId { get; set; }
    public AccountLink AccountLink { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    
    public int StartingPoint { get; set; }
    public int EndPoint { get; set; }
    public Collection<TrackableEventProgress> Progress { get; set; } = new();
}