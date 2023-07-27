using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

[assembly: FunctionsStartup(typeof(Bihyung.Functions.Startup))]

namespace Bihyung.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {

    }
}