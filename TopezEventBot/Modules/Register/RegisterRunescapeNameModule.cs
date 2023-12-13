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
    private readonly ILogger<RegisterRunescapeNameModule> _logger;

    public RegisterRunescapeNameModule(IRunescapeHiscoreHttpClient client, IDbContextFactory<TopezContext> contextFactory, ILogger<RegisterRunescapeNameModule> logger)
    {
        _client = client;
        _contextFactory = contextFactory;
        _logger = logger;
    }

    [SlashCommand("unlink-rsn", "Unlinks your runescape username from your discord account")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task UnlinkRunescapeAccount()
    {
        await DeferAsync(ephemeral: true);
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation("Unlinking runescape account for discord-member {globalName}", Context.User.GlobalName);
        var activeAccount = await ctx.AccountLinks.FirstOrDefaultAsync(x => x.DiscordMemberId == Context.User.Id && x.IsActive);
        if (activeAccount == null)
        {
            _logger.LogInformation("No active account found, aborting!");
            await FollowupAsync("You got no account or active accounts linked to your discord id. " +
                                                   "Please Link your runescape accout with the */link-rsn* command", ephemeral: true);
            return;
        }

        activeAccount.IsActive = false;
        ctx.AccountLinks.Update(activeAccount);
        var deleted = await ctx.SaveChangesAsync();

        if (deleted > 0) _logger.LogInformation("Deactivating account \"{accountName}\"!", activeAccount.RunescapeName);
        else _logger.LogInformation("Could not deactivate account \"{accountName}\"", activeAccount.RunescapeName);

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
        _logger.LogInformation("Starting confirmation process for user {userName}", modal.AccountName);
        var rsn = modal.AccountName;
        var player = await _client.LoadPlayer(rsn);
        if (player != null) _logger.LogInformation("Successfully fetched player information for rsn {accountName}!", modal.AccountName);
        else {
            _logger.LogInformation("Fetching player information for rsn {accountName} failed!", modal.AccountName);
            await FollowupAsync(text: "Fetching hiscore stats failed, aborting!", ephemeral: true);
            return;
        }
        var componentBuilder = new ComponentBuilder()
            .WithButton("That's me", $"confirm-rsn-button:{rsn}", ButtonStyle.Success)
            .WithButton("Nope, not me!", $"not-me-button:{rsn}", ButtonStyle.Danger);

        await FollowupAsync(embed: EmbedGenerator.Player(player!), text: "Is this you?",
            components: componentBuilder.Build(), ephemeral: true);
    }

    [ComponentInteraction("confirm-rsn-button:*")]
    public async Task ConfirmUsernameButton(string rsn)
    {
        await DeferAsync(ephemeral: true);
        _logger.LogInformation("Starting runescape name confirmation process for Discord Member {userName} with account {rsn}", Context.User.GlobalName, rsn);
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var memberId = Context.User.Id;
        var existingLinkedAccount = await ctx.AccountLinks.FirstOrDefaultAsync(x =>
            x.DiscordMemberId == memberId &&
            string.Equals(x.RunescapeName, rsn, StringComparison.CurrentCultureIgnoreCase));

        if (existingLinkedAccount is { IsActive: true })
        {
            _logger.LogInformation("Account with the same UserName \"{rsn}\" already linked and active", rsn);
            await FollowupAsync("This account is already your linked, active account!", ephemeral: true);
            return;
        }

        var linkedAccounts = ctx.AccountLinks.Where(x => x.DiscordMemberId == memberId);
        _logger.LogInformation("Setting other accounts inactive!");
        foreach (var linkedAccount in linkedAccounts)
        {
            linkedAccount.IsActive = false;
        }

        ctx.UpdateRange(linkedAccounts);
        await ctx.SaveChangesAsync();

        if (existingLinkedAccount == null)
        {
            _logger.LogInformation("No account with the same Username already linked, creating new linked account");
            ctx.AccountLinks.Add(new AccountLink { DiscordMemberId = memberId, RunescapeName = rsn, IsActive = true });
        }
        else
        {
            _logger.LogInformation("Account with the same Username already linked, setting it as the active account");
            existingLinkedAccount.IsActive = true;
            ctx.Update(existingLinkedAccount);
        }

        var count = await ctx.SaveChangesAsync();
        await FollowupAsync(count > 0
            ? $"Account {rsn} successfully linked!"
            : "Problem while linking account!", ephemeral: true);
    }
 
    [ComponentInteraction("not-me-button:*")]
    public async Task NotMeButtonResponse(string rsn)
    {
        await DeferAsync(ephemeral: true);
        _logger.LogInformation("Runescape account {rsn} is not {discordMembers} id!", rsn, Context.User.GlobalName);
        await FollowupAsync("So that's not your account, linking has been stopped!", ephemeral: true);
    }
}
