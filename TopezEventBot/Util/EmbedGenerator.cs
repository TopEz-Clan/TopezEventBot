using System.Reflection;
using Discord;
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
                    IconUrl = Constants.TOPEZ_LOGO_URL,
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

    
    public static Embed WildyWednesday(HiscoreField activity, String location, String startTime) {
        var description = "As always, *REACT* to this post if you're planning on coming so we have an idea on how many members are joining.";
        var title = $"Wildy Wednesday - {activity.GetDisplayName()}";
        var footer = new EmbedFooterBuilder { Text = "For non-pkers: Toggle PK Skull Prevention in settings! Only risk what you can afford to lose.",  IconUrl = Constants.INFORMATIONAL_FOOTER_LOGO_URL };
        var fields = new EmbedFieldBuilder[]{
                Field("This week's boss:", activity.GetDisplayName(), true),
                Field("Where to meet:", "*" + location + "*", true),
                Field("Event Start:", "*Wednesday @ " + startTime + " UTC*", true),
                Field("Splitting Rules:", "Only split voidwaker pieces, the rest is FFA", false),
        };
    
        return Generate(title, description, activity, footer, fields);
    }
    //
    // public static EmbedCreateSpec Mass(String location, String startTime) {
    //     var title = "Sunday Corp Mass";
    //     var description = "As always, *REACT* to this post if you're planning on coming so we have an idea on how many members are joining.";
    //     var data = new EmbedData("","https://i.imgur.com/kStnFWS.png","https://i.imgur.com/Uvk3Tot.png");
    //     var fields = new EmbedCreateFields.Field[] {
    //         EmbedCreateFields.Field.of("Where to meet:", "*" + location + "*", true),
    //         EmbedCreateFields.Field.of("Event Start:", "*Sunday @ " + startTime.substring(0, startTime.length() - 1) + " UTC*", true),
    //         EmbedCreateFields.Field.of("Splitting Rules:", "Split all Sigil drops between participants of the kill; Free For All on the rest of the drops.", false),
    //     };
    //     var footer = EmbedCreateFields.Footer.of("Check out #setups-and-guides if you're unsure what to bring to corp!", Constants.INFORMATIONAL_FOOTER_LOGO_URL);
    //     return Generate(title, description, HiscoreField.CorporealBeast, footer, fields); // todo
    // }
    //

    private static EmbedFieldBuilder Field(string name, string value, bool inline)
    {
        return new EmbedFieldBuilder { IsInline = inline, Name = name, Value = value };
    }
    
    public static Embed SkillOfTheWeek(HiscoreField skill, String codeWord) {
        var title = $"Skill of the Week - {skill.GetDisplayName()}";
        var description = "As always, the 1st place winner of the competition will receive a prize, as well as a temporary in-game rank. Be sure to post a screenshot below containing the code-word and your start exp in the skill! This event will last a week, so good luck!";
        var fields = new EmbedFieldBuilder[] {
                Field("This week's skill:", skill.GetDisplayName(), true) ,
                Field("This week's code-word:", $"*{codeWord}*",  true) ,
                Field("Prize:", "A bond for ironmen, or GP equivalent for a main account", false) ,
        };
        
        var footer = new EmbedFooterBuilder { Text = "Be sure to include the code-word in your starting screenshot or you will not be eligible to win!", IconUrl = Constants.INFORMATIONAL_FOOTER_LOGO_URL };
        return Generate(title, description, skill, footer, fields);
    }
    //
    public static Embed BossOfTheWeek(HiscoreField boss, String codeWord) {
        var title = $"Boss of the Week - {boss.GetDisplayName()}";
        var description = "As always, the 1st place winner of the competition will receive a prize, as well as a temporary in-game rank. Be sure to post a screenshot below including the code-word as well as your starting killcount! (Found in the collection log)";
        var footer = new EmbedFooterBuilder { Text = "Be sure to include the code-word in your starting screenshot or you will not be eligible to win!", IconUrl = Constants.INFORMATIONAL_FOOTER_LOGO_URL };
        var fields = new EmbedFieldBuilder[] {
                Field("This week's boss:", boss.GetDisplayName(), true),
                Field("The code-word:", $"*{codeWord}*", true),
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
           IconUrl = Constants.INFORMATIONAL_FOOTER_LOGO_URL,
        };
        var fields = player.Skills.Select(skill => new EmbedFieldBuilder { IsInline = true, Name = skill.Key.ToString(), Value = skill.Value.Level }).ToList();

        return Generate(title, description, HiscoreField.Prayer, footer, fields.ToArray());
    }
}