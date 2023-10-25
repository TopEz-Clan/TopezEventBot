using Coravel;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TopezEventBot;
using TopezEventBot.Data;
using TopezEventBot.Data.Context;
using TopezEventBot.Http;
using TopezEventBot.Invocables;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All
        });
        services.AddSingleton<DiscordSocketClient>();       // Add the discord client to services
        services.AddSingleton<InteractionService>();        // Add the interaction service to services
        services.AddTransient<CheckForScheduledEventNotification>();
        services.AddHostedService<DiscordStartupService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHttpClient<IRunescapeHiscoreHttpClient, RunescapeHiscoreHttpClient>(client =>
        {
            client.BaseAddress = new Uri("https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws");
        });
        services.AddScheduler();
        
        services.AddDbContext<TopezContext>(opts => opts.UseSqlite($"Data Source={Util.DbPath()}"));
    })
    .Build();

host.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<CheckForScheduledEventNotification>()
        .EveryMinute();

});

await UpdateDatabase(host);


host.Run();
return;

async Task UpdateDatabase(IHost host)
{
    using var scope = host.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetService<TopezContext>();
    await ctx?.Database.MigrateAsync();
}