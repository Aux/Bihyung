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
        


        await context.Response.WriteAsync("Hello, Functions Framework.");
    }
}
