using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace TopezEventBot;

public class InteractionHandlingService : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly InteractionService _interactions;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;
    private readonly ILogger<InteractionService> _logger;

    public InteractionHandlingService(
        DiscordSocketClient discord,
        InteractionService interactions,
        IServiceProvider services,
        IConfiguration config,
        ILogger<InteractionService> logger)
    {
        _discord = discord;
        _interactions = interactions;
        _services = services;
        _config = config;
        _logger = logger;

        _interactions.Log += msg => LogHelper.OnLogAsync(_logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _discord.Ready += () => _interactions.RegisterCommandsGloballyAsync(true);
        _discord.InteractionCreated += OnInteractionAsync;

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _interactions.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnInteractionAsync(SocketInteraction interaction)
    {
        try
        {
            var scope = _services.CreateScope();
            var context = new SocketInteractionContext(_discord, interaction);
            var result = await _interactions.ExecuteCommandAsync(context, scope.ServiceProvider);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ToString());
        }
        catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(msg => msg.Result.DeleteAsync());
            }
        }
    }
}
public static class LogHelper
{
    public static Task OnLogAsync(ILogger logger, LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Verbose:
                logger.LogInformation(msg.ToString());
                break;

            case LogSeverity.Info:
                logger.LogInformation(msg.ToString());
                break;

            case LogSeverity.Warning:
                logger.LogWarning(msg.ToString());
                break;

            case LogSeverity.Error:
                logger.LogError(msg.ToString());
                break;

            case LogSeverity.Critical:
                logger.LogCritical(msg.ToString());
                break;
        }
        return Task.CompletedTask;
    }
}