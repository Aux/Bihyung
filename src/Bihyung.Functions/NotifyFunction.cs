using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bihyung.Functions;

/// <summary>
///     Container for <see cref="Bihyung.Services.NotifyService"/> when executed as a cloud function.
/// </summary>
public class NotifyFunction : IHttpFunction
{
    public async Task HandleAsync(HttpContext context)
    {
        await context.Response.WriteAsync("Hello, Functions Framework.");
    }
}
