using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;

namespace TopezEventBot.Modules.Admin;

[Group("admin", "Administration-related modules")]
public class AdminModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AdminModule(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    [RequireRole("Coordinator")]
    [SlashCommand( "linked-accounts", "Gets a list of all users with linked runescape accounts")]
    public async Task GetLinkedUsers()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();

        await DeferAsync(ephemeral: true);
        var linkedAccounts = await db.AccountLinks.ToListAsync();

        var result = $"Currently there are {linkedAccounts.Count()} linked accounts: \n";
        await FollowupAsync(linkedAccounts.Aggregate(result,
            ((current, link) => current + MentionUtils.MentionUser(link.DiscordMemberId) +
                                $" with rsn {link.RunescapeName}\n")), ephemeral: true);
    }
    
}