using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Model;
using TopezEventBot.Http;
using TopezEventBot.Util;

namespace TopezEventBot.Modules;

public class RegisterRunescapeNameModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IRunescapeHiscoreHttpClient _client;
    private readonly IServiceProvider _provider;

    public RegisterRunescapeNameModule(IRunescapeHiscoreHttpClient client, IServiceProvider provider)
    {
        _client = client;
        _provider = provider;
    }
    
    [SlashCommand("unlink-rsn", "Unlinks your runescape username from your discord account")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task UnlinkRunescapeAccount() {
        using var scope = _provider.CreateScope();
        await using var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var account = await ctx.AccountLinks.FirstOrDefaultAsync(x => x.DiscordMemberId == Context.User.Id);
        if (account == null)
        {
            await Context.Interaction.RespondAsync("You got no account linked to your discord id. " +
                                                   "Please Link your runescape accout with the */link-rsn* command", ephemeral: true);
            return;
        }

        var unlinkedAccount = ctx.AccountLinks.Remove(account);
        var deleted = await ctx.SaveChangesAsync();

        await Context.Interaction.RespondAsync(deleted > 0
            ? $"Unlinked Account {unlinkedAccount.Entity.RunescapeName} "
            : "There was an error unlinking your runescape account, please try again in a few seconds", ephemeral: true);
    }
    
    

    [SlashCommand("link-rsn", "Link your runescape name!")]
    [RequireUserPermission(GuildPermission.ViewChannel)]
    public async Task RegisterRunescapeName()
    {
        using var scope = _provider.CreateScope();
        await using var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var memberId = Context.User.Id;
        var accountLinked = ctx.AccountLinks.Any(x => x.DiscordMemberId == memberId);

        if (accountLinked)
            await Context.Interaction.RespondAsync("You already got a linked account!", ephemeral: true);
        else
            await Context.Interaction.RespondWithModalAsync<RegisterRunescapeNameModal>("link_rsn_modal");
    }


    [ModalInteraction("link_rsn_modal")]
    public async Task LinkModalResponse(RegisterRunescapeNameModal modal)
    {
        var rsn = modal.AccountName;
        var player = await _client.LoadPlayer(rsn);
        var componentBuilder = new ComponentBuilder()
            .WithButton("That's me", $"confirm-rsn-button:{rsn}", ButtonStyle.Success)
            .WithButton("Nope, not me!", "not-me-button", ButtonStyle.Danger);

        await RespondAsync(embed: EmbedGenerator.Player(player), text: "Is this you?",
            components: componentBuilder.Build(), ephemeral: true);
    }

    [ComponentInteraction("confirm-rsn-button:*")]
    public async Task ConfirmUsernameButton(string rsn)
    {
        using var scope = _provider.CreateScope();
        var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var member = Context.User.Id;
        ctx?.AccountLinks.Add(new AccountLink { DiscordMemberId = member, RunescapeName = rsn });
        var count = await ctx.SaveChangesAsync();
        await Context.Interaction.RespondAsync(count > 0
            ? $"Account {rsn} successfully linked!"
            : "Problem while linking account!", ephemeral: true);
    }

    [ComponentInteraction("not-me-button")]
    public async Task NotMeButtonResponse()
    {
        await Context.Interaction.RespondAsync("So that's not your account, linking has been stopped!");
    }
}