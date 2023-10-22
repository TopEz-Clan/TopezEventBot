using Discord;
using Discord.Interactions;
using Discord.WebSocket;
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

    [SlashCommand("link-rsn", "Link your runescape name!")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task RegisterRunescapeName()
    {
        using var scope = _provider.CreateScope();
        await using var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var memberId = Context.User.Id;
        var accountLinked = ctx.AccountLinks.Any(x => x.DiscordMemberId == memberId);

        if (accountLinked)
            await Context.Interaction.RespondAsync("You already got a linked account!");
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
        
        await RespondAsync(embed: EmbedGenerator.Player(player), text: "Is this you?", components: componentBuilder.Build());
    }

    [ComponentInteraction("confirm-rsn-button:*")]
    public async Task ConfirmUsernameButton(string rsn)
    {
        using var scope = _provider.CreateScope();
        var ctx = scope.ServiceProvider.GetService<TopezContext>();
        var member = Context.User.Id;
        ctx?.AccountLinks.Add(new AccountLink { DiscordMemberId = member, RunescapeName = rsn});
        var count = await ctx.SaveChangesAsync();
        if (count > 0)
            await Context.Interaction.RespondAsync($"Account {rsn} successfully linked!");
        else
            await Context.Interaction.RespondAsync("Problem while linking account!");
    }
    
    [ComponentInteraction("not-me-button")]
    public async Task NotMeButtonResponse()
    {
        await Context.Interaction.RespondAsync("Not me!");
    }
}