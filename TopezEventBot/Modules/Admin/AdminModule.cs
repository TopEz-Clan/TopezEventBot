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
    [SlashCommand("linked-accounts", "Gets a list of all users with linked runescape accounts")]
    public async Task GetLinkedUsers()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();

        await DeferAsync(ephemeral: true);
        var linkedAccounts = await db.AccountLinks.AsNoTracking().ToListAsync();

        var result = $"Currently there are {linkedAccounts.Count()} linked accounts: \n";
        await FollowupAsync(linkedAccounts.Aggregate(result,
            ((current, link) => current + MentionUtils.MentionUser(link.DiscordMemberId) +
                                $" with rsn {link.RunescapeName} - active: {link.IsActive}\n")), ephemeral: true);
    }

    [RequireRole("Management")]
    [SlashCommand("delete-user-link", "unlink a user account by force")]
    public async Task DeleteUserLink(long internalUserId)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();

        await DeferAsync(ephemeral: true);
        var user = db.AccountLinks.FirstOrDefault(x => x.Id == internalUserId);
        if (user == null)
        {
            await FollowupAsync("This account doesn't exist in the database!");
            return;
        }
        db.AccountLinks.Remove(user!);
        await db.SaveChangesAsync();
        await FollowupAsync($"Removed AccountLink for user {user.RunescapeName}", ephemeral: true);
    }

}
