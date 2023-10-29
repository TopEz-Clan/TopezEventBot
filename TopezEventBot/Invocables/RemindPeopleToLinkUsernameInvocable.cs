using Coravel.Invocable;
using Discord.WebSocket;
using TopezEventBot.Data.Context;

namespace TopezEventBot.Invocables;

public class RemindPeopleToLinkUsernameInvocable : IInvocable
{
    private readonly TopezContext _db;
    private readonly DiscordSocketClient _client;

    public RemindPeopleToLinkUsernameInvocable(TopezContext db, DiscordSocketClient client)
    {
        _db = db;
        _client = client;
    }
    
    public Task Invoke()
    {
        var guild = _client.Guilds.FirstOrDefault();
        var registeredMembers = _db.AccountLinks.Select(x => x.DiscordMemberId).ToList();
        var notLinkedMembers = guild.Users.Select(x => x.Id).Except(registeredMembers);

        foreach (var unregisteredMember in notLinkedMembers)
        {
        }
        
        throw new NotImplementedException();
    }
}