using Coravel.Invocable;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Util;
using TopezEventBot.Util.Extensions;

namespace TopezEventBot.Invocables
{
    public class CheckForScheduledEventNotification : IInvocable
    {
        private readonly IDbContextFactory<TopezContext> _ctxFactory;
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger<CheckForScheduledEventNotification> _logger;

        public CheckForScheduledEventNotification(IDbContextFactory<TopezContext> ctxFactory, DiscordSocketClient discordClient, ILogger<CheckForScheduledEventNotification> logger)
        {
            _ctxFactory = ctxFactory;
            _discordClient = discordClient;
            _logger = logger;
        }

        public async Task Invoke()
        {
            _logger.LogWarning("Started invocation!");
            // What is your invocable going to do?
            var startRange = DateTimeOffset.UtcNow.AddMinutes(-30);
            var endRange = DateTimeOffset.UtcNow.AddMinutes(30);
            await using var ctx = await _ctxFactory.CreateDbContextAsync();
            var scheduledEventsInTimeFrame =
                ctx.SchedulableEvents
                .Include(x => x.EventParticipations)
                .ThenInclude(x => x.AccountLink)
                .AsEnumerable()
                .Where(x => x.ScheduledAt.UtcDateTime >= startRange.UtcDateTime && x.ScheduledAt.UtcDateTime < endRange.UtcDateTime);

            foreach (var @event in scheduledEventsInTimeFrame)
            {
                var unnotifiedParticipants = @event.EventParticipations.Where(x => !x.Notified);
                foreach (var participant in unnotifiedParticipants)
                {
                    var user = _discordClient.GetUser(participant.AccountLink.DiscordMemberId);
                    await user.SendMessageAsync(embed: EmbedGenerator.ScheduledEventReminder(@event));

                    participant.Notified = true;
                }
                ctx.UpdateRange(unnotifiedParticipants);
            }

            await ctx.SaveChangesAsync();
        }
    }
}
