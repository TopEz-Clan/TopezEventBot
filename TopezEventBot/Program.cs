using Coravel;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TopezEventBot;
using TopezEventBot.Data.Context;
using TopezEventBot.Http;
using TopezEventBot.Invocables;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All,
            UseInteractionSnowflakeDate = false
        });
        services.AddSingleton(new InteractionServiceConfig()
        {
            UseCompiledLambda = true,
        });
        services.AddSingleton<DiscordSocketClient>();       // Add the discord client to services
        services.AddSingleton<InteractionService>();        // Add the interaction service to services
        services.AddTransient<CheckForScheduledEventNotification>();
        services.AddTransient<FetchEventProgressInvocable>();
        services.AddHostedService<DiscordStartupService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHttpClient<IRunescapeHiscoreHttpClient, RunescapeHiscoreHttpClient>(client =>
        {
            client.BaseAddress = new Uri("https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws");
        });
        services.AddScheduler();
        
        services.AddDbContext<TopezContext>((services, opts) =>
        {
            var config = services.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("topez");
            if (connectionString.Contains("%LOCALAPPDATA%"))
            {
                connectionString = connectionString.Replace("%LOCALAPPDATA%",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
            opts.UseSqlite(connectionString);
        });
    })
    .Build();

host.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<CheckForScheduledEventNotification>()
        .EveryMinute();

    scheduler.Schedule<FetchEventProgressInvocable>()
        .EveryMinute().Once();
});

await UpdateDatabase(host);

host.Run();
return;

async Task UpdateDatabase(IHost host)
{
    await using var scope = host.Services.CreateAsyncScope();
    var ctx = scope.ServiceProvider.GetRequiredService<TopezContext>();
    await ctx.Database.MigrateAsync();
}