using Discord;
using Discord.Interactions;
using TopezEventBot.Http;
using TopezEventBot.Util;

namespace TopezEventBot.Modules;

public class RegisterRunescapeNameModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IRunescapeHiscoreHttpClient _client;

    public RegisterRunescapeNameModule(IRunescapeHiscoreHttpClient client)
    {
        _client = client;
    }

    [SlashCommand("link-rsn", "Link your runescape name!")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task RegisterRunescapeName()
    {
        await Context.Interaction.RespondWithModalAsync<RegisterRunescapeNameModal>("link_rsn_modal");
    }


    [ModalInteraction("link_rsn_modal")]
    public async Task LinkModalResponse(RegisterRunescapeNameModal modal)
    {
        var rsn = modal.AccountName;
        var player = await _client.LoadPlayer(rsn);
        var componentBuilder = new ComponentBuilder()
            .WithButton("That's me", "confirm-rsn-button", ButtonStyle.Success)
            .WithButton("Nope, not me!", "not-me-button", ButtonStyle.Danger);
        await RespondAsync(embed: EmbedGenerator.Player(player), text: "Is this you?", components: componentBuilder.Build());
    }

    [ComponentInteraction("confirm-rsn-button")]
    public async Task ConfirmUsernameButton()
    {
        var initial = await Context.Interaction.GetOriginalResponseAsync();
        await Context.Interaction.RespondAsync("Success!");
    }
    
    [ComponentInteraction("not-me-button")]
    public async Task NotMeButtonResponse()
    {
        await Context.Interaction.RespondAsync("Not me!");
    }
}