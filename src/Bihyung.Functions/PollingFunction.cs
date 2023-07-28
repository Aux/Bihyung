using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bihyung.Functions;

/// <summary>
///     Cloud Function used for "asynchronous" polling of configured webcomics
/// </summary>
/// <remarks>
///     Polling is executed once per configured webcomic. 
/// </remarks>
public class PollingFunction : IHttpFunction
{
    public async Task HandleAsync(HttpContext context)
    {
        // Accepts a webcomic as a body parameter
        // Sends a request to the polling url
        // Checks the polling type and creates a PollingStatus
        // If success, trigger Notify and return 201
        // If unmodified, return 204
        // If failure, return 500 with error body

        await context.Response.WriteAsync("Hello, Functions Framework.");
    }
}
