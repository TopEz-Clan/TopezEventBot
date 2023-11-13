using System.Reflection;
using Discord;
using TopezEventBot.Data.Entities;
using TopezEventBot.Data.Models.Extensions;
using TopezEventBot.Http.Models;
using TopezEventBot.Modules.WildyWednesday;
using TopezEventBot.Util.Extensions;

namespace TopezEventBot.Util;
public class EmbedGenerator {

    private static Embed Generate(string title, string description, HiscoreField data, EmbedFooterBuilder footer, params EmbedFieldBuilder[] fields)
    {
        var specBuilder = new EmbedBuilder()
            {
                Color = new Color(45,173,223),
                Author = new EmbedAuthorBuilder()
                {
                    Name = "TopEz Staff Team",
                    IconUrl = Constants.TopezLogoUrl,
                    Url = null
                },
                Title = title,
                Description = description,
                ThumbnailUrl = data.GetThumbnailUrl()
            };

        foreach (var field in fields)
        {
            specBuilder.AddField(field);
        }

        specBuilder.Footer = footer;
        specBuilder.Timestamp = DateTimeOffset.Now;
        specBuilder.ImageUrl = data.GetBannerUrl();
        return specBuilder.Build();
    }
    
    public static Embed WildyWednesday(HiscoreField activity, string location, DateTime startTime) {
        var description = "As always, use the *REGISTER* button down below if you're planning on coming so we have an idea on how many members are joining.";
        var title = $"Wildy Wednesday - {activity.GetDisplayName()}";
        var footer = new EmbedFooterBuilder { Text = "For non-pkers: Toggle PK Skull Prevention in settings! Only risk what you can afford to lose.",  IconUrl = Constants.InformationalFooterLogoUrl };
        var fields = new EmbedFieldBuilder[]{
                Field("This week's boss:", activity.GetDisplayName(), true),
                Field("Where to meet:", "*" + location + "*", true),
                Field("Event Start:", $"*{startTime:f} UTC*", true),
                Field("Splitting Rules:", "Only split voidwaker pieces, the rest is FFA", false),
        };
    
        return Generate(title, description, activity, footer, fields);
    }
    
    public static Embed Mass(HiscoreField activity, string location, DateTime startTime) {
        var title = $"{activity.GetDisplayName()} Mass";
        var description = "As always, click the register button on this post if you're planning on coming so we have an idea on how many members are joining.";
        var fields = new[] {
            Field("Where to meet:", "*" + location + "*", true),
            Field("Event Start:", $"*{startTime.ToUniversalTime():f} UTC*", true),
            Field("Splitting Rules:", "Split all big drops between participants of the kill; Free For All on the rest of the drops.", false),
        };
        var footer = new EmbedFooterBuilder { Text = "Check out #setups-and-guides if you're unsure what to bring!", IconUrl = Constants.InformationalFooterLogoUrl};
        return Generate(title, description, activity, footer, fields); 
    }
    

    private static EmbedFieldBuilder Field(string name, string value, bool inline)
    {
        return new EmbedFieldBuilder { IsInline = inline, Name = name, Value = value };
    }
    
    public static Embed ScheduledEventReminder(SchedulableEvent @event)
    {
        var title = $"Event reminder";
        var description =
            $"Your event starts in {( @event.ScheduledAt.UtcDateTime - DateTimeOffset.UtcNow.UtcDateTime ).Minutes} Minutes";
        
        var fields = new EmbedFieldBuilder[] {
           Field("Activity", @event.Activity.GetDisplayName(), inline: true),
           Field("Location", @event.Location, inline: true),
           Field("Start time", @event.ScheduledAt.UtcDateTime.ToString("f") + " UTC", inline: true),
        };
        var footer = new EmbedFooterBuilder()
        {
            IconUrl = Constants.InformationalFooterLogoUrl,
            Text = "Please head over to the specified location"
        };
        return Generate(title, description, @event.Activity, footer, fields);
    }
    
    public static Embed SkillOfTheWeek(HiscoreField skill) {
        var title = $"Skill of the Week - {skill.GetDisplayName()}";
        var description = "As always, the 1st place winner of the competition will receive a prize, as well as a temporary in-game rank! This event will last a week, so good luck!";
        var fields = new EmbedFieldBuilder[] {
                Field("This week's skill:", skill.GetDisplayName(), true) ,
                Field("Prize:", "A bond for ironmen, or GP equivalent for a main account", false) ,
        };
        
        var footer = new EmbedFooterBuilder { Text = "Be sure to include the code-word in your starting screenshot or you will not be eligible to win!", IconUrl = Constants.InformationalFooterLogoUrl };
        return Generate(title, description, skill, footer, fields);
    }
    //
    public static Embed BossOfTheWeek(HiscoreField boss) {
        var title = $"Boss of the Week - {boss.GetDisplayName()}";
        var description = "As always, the 1st place winner of the competition will receive a prize, as well as a temporary in-game rank. This event will last a week, so good luck!";
        var footer = new EmbedFooterBuilder { Text = "Be sure to include the code-word in your starting screenshot or you will not be eligible to win!", IconUrl = Constants.InformationalFooterLogoUrl };
        var fields = new EmbedFieldBuilder[] {
                Field("This week's boss:", boss.GetDisplayName(), true),
                Field("Prize:", "A bond for ironmen, or the GP equivalent for a main account", false),
        };
        return Generate(title, description, boss, footer, fields);
    }

    public static Embed Player(Player player) {
        var title = "Overview for "+ player.UserName;
        var description = "As always, the 1st place winner of the competition will receive a prize, as well as a temporary in-game rank. Be sure to post a screenshot below including the code-word as well as your starting killcount! (Found in the collection log)";
        var footer = new EmbedFooterBuilder
        {
            Text = "Be sure to include the code-word in your starting screenshot or you will not be eligible to win!",
           IconUrl = Constants.InformationalFooterLogoUrl,
        };
        var fields = player.Skills.Select(skill => new EmbedFieldBuilder { IsInline = true, Name = skill.Key.ToString(), Value = skill.Value.Level }).ToList();

        return Generate(title, description, HiscoreField.Prayer, footer, fields.ToArray());
    }
    
    public static Embed EventWinner(TrackableEventType type, HiscoreField activity, IEnumerable<EventResult> winners) {
        var title = $"Winner of this Weeks {type.GetDisplayName()} - @{winners.FirstOrDefault().RunescapeName}";
        var description = "As always, the 1st place winner of the competition will receive a prize, as well as a temporary in-game rank";
        var footer = new EmbedFooterBuilder
        {
            Text = "Congratulation! Hit up the Mod Team to claim your price",
           IconUrl = Constants.InformationalFooterLogoUrl,
        };
        var idx = 0;

        return Generate(title, description, activity, footer, winners.Select(winner => new EmbedFieldBuilder() { IsInline = false, Name = idx < 3 ? $"{++idx}. Place" : throw new ArgumentException(), Value = $"{winner.RunescapeName} - {winner.Progress} {type.Unit()}" }).ToArray());
    }
}