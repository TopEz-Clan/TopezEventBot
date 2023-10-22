using Discord;
using Discord.WebSocket;
using TopezEventBot.Http;

namespace TopezEventBot;

public class DiscordStartupService : IHostedService
{
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly IConfiguration _config;
    private readonly ILogger<DiscordStartupService> _logger;
    private readonly IRunescapeHiscoreHttpClient client;

    
    public DiscordStartupService(ILogger<DiscordStartupService> logger, DiscordSocketClient discordSocketClient, IConfiguration config, IRunescapeHiscoreHttpClient client)
    {
        _logger = logger;
        _discordSocketClient = discordSocketClient;
        _config = config;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _discordSocketClient.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("BOT_TOKEN"));
        await _discordSocketClient.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discordSocketClient.LogoutAsync();
        await _discordSocketClient.StopAsync();
    }

}