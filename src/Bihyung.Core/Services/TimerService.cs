using Microsoft.Extensions.Hosting;

namespace Bihyung.Services;

/// <summary>
///     Pulls webcomics from the database when they're due for polling and runs them.
/// </summary>
public class TimerService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task StopAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
}
