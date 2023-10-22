using Discord;
using Discord.Interactions;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.SkillOfTheWeek;

public class SkillOfTheWeekModule : InteractionModuleBase<SocketInteractionContext>
{
    
    [SlashCommand("start-sotw", "Start skill of the week!")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task StartSkillOfTheWeek(SkillOfTheWeekChoice sotw, string codePhrase)
    {
        await RespondAsync(embed: EmbedGenerator.SkillOfTheWeek((HiscoreField)sotw, codePhrase));
    }
    
}