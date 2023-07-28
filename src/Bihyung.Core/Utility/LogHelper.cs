using Discord;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Bihyung;

public static class LogHelper
{
    public static Task OnLogAsync(ILogger logger, LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Verbose:
                logger.ZLogInformation(msg.ToString());
                break;

            case LogSeverity.Info:
                logger.ZLogInformation(msg.ToString());
                break;

            case LogSeverity.Warning:
                logger.ZLogWarning(msg.ToString());
                break;

            case LogSeverity.Error:
                logger.ZLogError(msg.ToString());
                break;

            case LogSeverity.Critical:
                logger.ZLogCritical(msg.ToString());
                break;
        }
        return Task.CompletedTask;
    }
}