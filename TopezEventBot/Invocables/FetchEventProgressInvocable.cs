using Coravel.Invocable;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;

namespace TopezEventBot.Invocables;

public class FetchEventProgressInvocable : IInvocable
{
    private readonly TopezContext _db;
    private readonly DiscordSocketClient _client;
    private readonly IRunescapeHiscoreHttpClient _hiscoreClient;

    public FetchEventProgressInvocable(TopezContext db, DiscordSocketClient client, IRunescapeHiscoreHttpClient hiscoreClient)
    {
        _db = db;
        _client = client;
        _hiscoreClient = hiscoreClient;
    }

    public async Task Invoke()
    {
        var activeEvents = _db.TrackableEvents
            .Include(x => x.EventParticipations)
            .ThenInclude(x => x.AccountLink)
            .Include(x => x.EventParticipations)
            .ThenInclude(x => x.Progress)
            .Where(x => x.IsActive);
        
        foreach (var activeEvent in activeEvents)
        {
            foreach (var participant in activeEvent.EventParticipations)
            {
                var hiscoreData = await _hiscoreClient.LoadPlayer(participant.AccountLink.RunescapeName);
                participant.Progress.Add(new TrackableEventProgress()
                {
                    FetchedAt = DateTime.Now,
                    Progress = activeEvent.Type switch {
                        TrackableEventType.BossOfTheWeek => hiscoreData.Bosses[activeEvent.Activity].KillCount == -1 ? 0 : hiscoreData.Bosses[activeEvent.Activity].KillCount,
                        TrackableEventType.SkillOfTheWeek => hiscoreData.Skills[activeEvent.Activity].Experience == -1 ? 0 : hiscoreData.Skills[activeEvent.Activity].Experience,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                });
                _db.Update(participant);
            }
        }

        await _db.SaveChangesAsync();
    }
}