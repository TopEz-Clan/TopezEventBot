using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.Register;

public class RegisterRunescapeNameModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IRunescapeHiscoreHttpClient _client;
    private readonly IDbContextFactory<TopezContext> _contextFactory;

    public RegisterRunescapeNameModule(IRunescapeHiscoreHttpClient client, IDbContextFactory<TopezContext> contextFactory)
    {
        _client = client;
        _contextFactory = contextFactory;
    }
    
    [SlashCommand("unlink-rsn", "Unlinks your runescape username from your discord account")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task UnlinkRunescapeAccount()
    {
        await DeferAsync(ephemeral: true);
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var activeAccount = await ctx.AccountLinks.FirstOrDefaultAsync(x => x.DiscordMemberId == Context.User.Id && x.IsActive);
        if (activeAccount == null)
        {
            await FollowupAsync("You got no account or active accounts  to your discord id. " +
                                                   "Please Link your runescape accout with the */link-rsn* command", ephemeral: true);
            return;
        }

        activeAccount.IsActive = false;
        ctx.AccountLinks.Update(activeAccount);
        var deleted = await ctx.SaveChangesAsync();

        await FollowupAsync(deleted > 0
            ? $"Set Account {activeAccount.RunescapeName} inactive"
            : "There was an error unlinking your runescape account, please try again in a few seconds", ephemeral: true);
    }

    [SlashCommand("link-rsn", "Link your runescape name!")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task RegisterRunescapeName()
    {
        await RespondWithModalAsync<RegisterRunescapeNameModal>("link_rsn_modal");
    }


    [ModalInteraction("link_rsn_modal")]
    public async Task LinkModalResponse(RegisterRunescapeNameModal modal)
    {
        await DeferAsync(ephemeral: true);
        var rsn = modal.AccountName;
        var player = await _client.LoadPlayer(rsn);
        var componentBuilder = new ComponentBuilder()
            .WithButton("That's me", $"confirm-rsn-button:{rsn}", ButtonStyle.Success)
            .WithButton("Nope, not me!", "not-me-button", ButtonStyle.Danger);

        await FollowupAsync(embed: EmbedGenerator.Player(player), text: "Is this you?",
            components: componentBuilder.Build(), ephemeral: true);
    }

    [ComponentInteraction("confirm-rsn-button:*")]
    public async Task ConfirmUsernameButton(string rsn)
    {
        await DeferAsync(ephemeral: true);
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var memberId = Context.User.Id;
        var existingLinkedAccount = await ctx.AccountLinks.FirstOrDefaultAsync(x =>
            x.DiscordMemberId == memberId &&
            string.Equals(x.RunescapeName, rsn, StringComparison.CurrentCultureIgnoreCase) && x.IsActive);
        
        if (existingLinkedAccount is { IsActive: true })
        {
            await FollowupAsync("This account is already your linked, active account!", ephemeral: true);
            return;
        }

        var linkedAccounts = ctx.AccountLinks.Where(x => x.DiscordMemberId == memberId);
        foreach (var linkedAccount in linkedAccounts)
        {
            linkedAccount.IsActive = false;
        }
        
        ctx.UpdateRange(linkedAccounts);
        if (existingLinkedAccount == null)
        {
            ctx.AccountLinks.Add(new AccountLink { DiscordMemberId = memberId, RunescapeName = rsn, IsActive = true});
        }
        else
        {
            existingLinkedAccount.IsActive = true;
            ctx.Update(existingLinkedAccount);
        }
        
        var count = await ctx.SaveChangesAsync();
        await FollowupAsync(count > 0
            ? $"Account {rsn} successfully linked!"
            : "Problem while linking account!", ephemeral: true);
    }

    [ComponentInteraction("not-me-button")]
    public async Task NotMeButtonResponse()
    {
        await RespondAsync("So that's not your account, linking has been stopped!");
    }
}