using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Context;
using TopezEventBot.Data.Entities;
using TopezEventBot.Data.Models.Extensions;
using TopezEventBot.Util;
using ComponentBuilder = Discord.ComponentBuilder;

namespace TopezEventBot.Modules;

public abstract class SchedulableEventModuleBase : InteractionModuleBase<SocketInteractionContext>
{
    protected readonly IServiceScopeFactory _scopeFactory;
    private readonly SchedulableEventType _type;

    protected SchedulableEventModuleBase(IServiceScopeFactory scopeFactory, SchedulableEventType type)
    {
        _scopeFactory = scopeFactory;
        _type = type;
    }

    protected async Task ScheduleEvent(HiscoreField activity, string location, DateTime time)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();


        await DeferAsync();

        var @event = await db.SchedulableEvents.AddAsync(new Data.Entities.SchedulableEvent()
        {
            Type = _type,
            Activity = activity,
            ScheduledAt = time,
            Location = location
        });

        await db.SaveChangesAsync();

        var registerButtonCustomId =
            $"register-for-{_type.GetShortIdentifier()}:{@event.Entity.Id}";


        var listParticipantsBtnCustomId = $"list-participants-{_type.GetShortIdentifier()}:{@event.Entity.Id}";
        
        var components = new ComponentBuilder().WithButton("Register", registerButtonCustomId, ButtonStyle.Success).WithButton("List participants", listParticipantsBtnCustomId, ButtonStyle.Secondary);
        await FollowupAsync(embed: GetEmbed(activity, location, time), components: components.Build());
    }
    
    protected abstract Embed GetEmbed(HiscoreField activity, string location, DateTime time);

    public abstract Task HandleRegistration(long eventId);
    
    public abstract Task ListParticipants(long eventId);

    protected async Task HandleEventRegistration(long eventId)
    {
        await DeferAsync();
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();
        var schedulableEvent = await db.SchedulableEvents
            .Include(x => x.EventParticipations)
            .ThenInclude(x => x.AccountLink)
            .FirstOrDefaultAsync(x => x.Id == eventId);
        if (schedulableEvent == null)
        {
            await FollowupAsync("The event you're trying to register for has been deleted!");
            return;
        }

        if (schedulableEvent.EventParticipations.Any(x => x.AccountLink.DiscordMemberId == Context.User.Id))
        {
            await FollowupAsync("You're already registered for this event!");
            return;
        }

        var accountLink = db.AccountLinks.FirstOrDefault(x => x.DiscordMemberId == Context.User.Id);
        if (accountLink == null)
        {
            await FollowupAsync(
                "You haven't linked your runescape account yet. Please link it by using the ```/link-rsn``` command!");
            return;
        }

        schedulableEvent.EventParticipations.Add(new SchedulableEventParticipation()
        {
         AccountLink = accountLink,
        });

        db.SchedulableEvents.Update(schedulableEvent);

        var count = await db.SaveChangesAsync();
        if (count > 0)
        {
            await FollowupAsync("Registration successful!", ephemeral: true);
            return;
        }

        await FollowupAsync("There's been a problem with your registration, please try again later!");
    }

    protected async Task ListEventParticipants(long eventId)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<TopezContext>();

        await DeferAsync();
        var @event = db.SchedulableEvents.Include(x => x.Participants).FirstOrDefault(x => x.Id == eventId);

        if (@event == null)
        {
            await FollowupAsync("The event seems to be deleted", ephemeral: true);
            return;
        }

        if (!@event.Participants.Any())
        {
            await FollowupAsync("There's no participants for this event yet :(", ephemeral: true);
            return;
        }

        if (!@event.Participants.Select(x => x.DiscordMemberId).Contains(Context.User.Id))
        {
            await FollowupAsync("You need to be registered to see the participants!", ephemeral: true);
            return;
        }
        var msg = @event.Participants.Aggregate("The following people plan to participate: \n\n", (current, accountLink) => current + "* " + (accountLink.RunescapeName + "\n"));

        await FollowupAsync(msg, ephemeral: true);
    }
}