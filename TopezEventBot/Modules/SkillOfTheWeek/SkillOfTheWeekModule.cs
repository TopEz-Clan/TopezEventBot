using Discord;
using Discord.Interactions;
using TopezEventBot.Data.Entities;
using TopezEventBot.Http;
using TopezEventBot.Util;

namespace TopezEventBot.Modules.SkillOfTheWeek;

[Group("sotw", "All skill of the week related commands")]
public class SkillOfTheWeekModule : TrackableEventModuleBase
{
    public SkillOfTheWeekModule(IServiceScopeFactory scopeFactory, IRunescapeHiscoreHttpClient rsClient) : base(scopeFactory, rsClient, TrackableEventType.SkillOfTheWeek)
    {
    }
    
    [SlashCommand("start", "Start skill of the week!")]
    [RequireRole("Coordinator")]
    public async Task StartSkillOfTheWeek(SkillOfTheWeekChoice sotw, bool isActive = true)
    {
        await StartEvent((HiscoreField)sotw, isActive);
    }

    [ComponentInteraction("register-for-sotw:*,*", ignoreGroupNames:true)]
    public async Task RegisterForSotw(string eventIdAsString, string threadIdAsString)
    {
        await base.RegisterForEvent(eventIdAsString, threadIdAsString);
    }
}