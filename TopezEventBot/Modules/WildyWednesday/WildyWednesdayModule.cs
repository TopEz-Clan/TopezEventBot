using Discord;
using Discord.Interactions;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.WildyWednesday;

public class WildyWednesdayModule : InteractionModuleBase<SocketInteractionContext>
{
    
    [SlashCommand("start-ww", "Create a new wildy-wednesday event")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartWildyWednesday(WildyWednesdayActivityChoice activity, string location, DateTime time)
    {
        await RespondAsync(embed: EmbedGenerator.WildyWednesday((HiscoreField)activity, location, time.ToString()));
    }
}