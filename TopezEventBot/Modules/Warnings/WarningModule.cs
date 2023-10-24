using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;

namespace TopezEventBot.Modules.Warnings;

public class WarningModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public WarningModule(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    [SlashCommand("warn", "Warns the user and adds a strike")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task WarnUser(IUser user)
    {
        await RespondWithModalAsync<WarningModal>($"issue-warning-modal:{user.Id}");
    }

    [SlashCommand("set-warning-channel", "Sets the warning channel")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task SetWarningChannel(IChannel channel)
    {
        using var scope = _scopeFactory.CreateScope();
        await using var db = scope.ServiceProvider.GetService<TopezContext>();
        var guildId = Context.Guild.Id;
        var channelByGuild = await db.GuildWarningChannels.FirstOrDefaultAsync(x => x.GuildId == guildId);
        if (channelByGuild == null)
        {
            db.GuildWarningChannels.Add(new GuildWarningChannel()
            {
                GuildId = guildId,
                WarningChannelId = channel.Id
            });
        }
        else
        {
            channelByGuild.WarningChannelId = channel.Id;
            db.GuildWarningChannels.Update(channelByGuild);
        }

        await db.SaveChangesAsync();
        await RespondAsync($"New guild warnings channel set to {MentionUtils.MentionChannel(channel.Id)}",
            ephemeral: true);
    }

    [ModalInteraction("issue-warning-modal:*")]
    public async Task HandleModal(string userIdAsString, WarningModal modal)
    {
        using var scope = _scopeFactory.CreateScope();
        await using var db = scope.ServiceProvider.GetService<TopezContext>();
        var userId = ulong.Parse(userIdAsString);

        db.Warnings.Add(new Warning()
        {
            Reason = modal.Reason,
            WarnedBy = Context.User.Id,
            WarnedUser = userId
        });

        await db.SaveChangesAsync();

        var warnings = db.Warnings.Where(x => x.WarnedUser == userId);
        var warningCount = warnings.Count();
        if (warningCount < 3)
        {
            var user = Context.Guild.Users.FirstOrDefault(x => x.Id == userId);
            await user.SendMessageAsync(
                $"You've been issued a warning by {MentionUtils.MentionUser(Context.User.Id)} for the following reason ```{modal.Reason}```\n" +
                $"Your current warning count is {warningCount}/3. \n" +
                $"After your third warning, the mod team will be notified.");
            await RespondAsync("The user has been informed!", ephemeral: true);
            return;
        }

        var warningChannel = await db.GuildWarningChannels.FirstOrDefaultAsync(x => x.GuildId == Context.Guild.Id);
        if (warningChannel == null)
        {
            await RespondAsync(
                "No warning-channel defined, please set a warning channel first by using the command ```/set-warning-channel```",
                ephemeral: true);
        }

        var userToWarn = Context.Guild.Users.FirstOrDefault(x => x.Id == userId);
        var componentBuilder = new ComponentBuilder().AddRow(new ActionRowBuilder().WithButton( "Clear warnings",$"clear-warnings-btn:{userToWarn.Id}", ButtonStyle.Success));
        var channel = Context.Guild.GetTextChannel(warningChannel.WarningChannelId)
            .SendMessageAsync(BuildWarningMessage(warnings, userToWarn), components: componentBuilder.Build());
        await RespondAsync("The mod team has been informed!", ephemeral: true);
    }

    [ComponentInteraction("clear-warnings-btn:*")]
    public async Task ClearUserWarningsBtnHandler(string userIdAsString)
    {
        var userId = ulong.Parse(userIdAsString);
        var user = Context.Guild.Users.FirstOrDefault(x => x.Id == userId);
        await ClearUserWarnings(user);
    }

    [SlashCommand("clear-user-warnings", "Clears the selected user of his warnings")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task ClearUserWarningsCommand(IUser user)
    {
        await ClearUserWarnings(user);
    }
    
    private async Task ClearUserWarnings(IUser user) {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();

        var userWarnings = db.Warnings.Where(x => x.WarnedUser == user.Id);

        db.Warnings.RemoveRange(userWarnings);
        var deletedWarningCount = await db.SaveChangesAsync();
        
        if (deletedWarningCount == 0) {
            await RespondAsync("No warnings to clear!");
            return;
        }
        
        await user.SendMessageAsync(
            $"You've been cleared of {deletedWarningCount} warnings by {MentionUtils.MentionUser(Context.User.Id)}");
        await RespondAsync($"User was cleared of {deletedWarningCount} warnings", ephemeral: true);
    }

    private static string BuildWarningMessage(IEnumerable<Warning> warnings, IUser user)
    {
        var msg =
            $"Hey guys, the user {MentionUtils.MentionUser(user.Id)} has been issued the following warnings that should be manually reviewed\n";
        var idx = 0;
        msg =
            $"{warnings.Aggregate(msg, (current, w) => current + $"{++idx}. by {MentionUtils.MentionUser(w.WarnedBy)} - {w.Reason}\n")}\n";
        msg += "If you want to clear the user of his warnings, use the command ```/clear-user-warnings```";
        return msg;
    }
}