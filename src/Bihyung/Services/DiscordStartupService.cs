using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bihyung.Services;

public class DiscordStartupService : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;

    public DiscordStartupService(
        DiscordSocketClient discord, 
        IConfiguration config,
        LoggerFactory factory)
    {
        _discord = discord;
        _config = config;
        _logger = factory.CreateLogger("Discord");

        _discord.Log += msg => LogHelper.OnLogAsync(_logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _discord.LoginAsync(TokenType.Bot, _config["discord:token"]);
        await _discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discord.LogoutAsync();
        await _discord.StopAsync();
    }
}
