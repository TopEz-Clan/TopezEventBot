using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TopezEventBot;
using TopezEventBot.Data.Context;
using TopezEventBot.Http;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<DiscordSocketClient>();       // Add the discord client to services
        services.AddSingleton<InteractionService>();        // Add the interaction service to services
        services.AddHostedService<DiscordStartupService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHttpClient<IRunescapeHiscoreHttpClient, RunescapeHiscoreHttpClient>(client =>
        {
            client.BaseAddress = new Uri("https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws");
        });
        
        services.AddDbContext<TopezContext>(opts => opts.UseSqlite($"Data Source={Util.DbPath()}"));
    })
    .Build();

host.Run();