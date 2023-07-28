using Bihyung;
using Bihyung.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLogger;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddEnvironmentVariables("BIHYUNG_");
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddZLoggerConsole();
        logging.AddZLoggerRollingFile((dt, x) => $"logs/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log", x => x.ToLocalTime().Date, 1024);
    })
    .AddDiscord()
    .AddFirestore()
    .ConfigureServices(services =>
    {
        services.AddHostedService<DiscordStartupService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHttpClient<WebtoonScraper>();
        
    })
    .Build();

await host.RunAsync();
