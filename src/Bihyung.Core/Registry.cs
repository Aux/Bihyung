﻿using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bihyung;

public static class Registry
{
    public static IHostBuilder AddDiscord(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient<DiscordRestClient>();
            services.AddSingleton(_ => new DiscordSocketClient(new()
            {
                AlwaysDownloadUsers = false,
                GatewayIntents = GatewayIntents.None,
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 0,
                SuppressUnknownDispatchWarnings = true
            }));
            services.AddSingleton(services =>
            {
                var discord = services.GetRequiredService<DiscordSocketClient>();
                return new InteractionService(discord, new()
                {
                    LogLevel = LogSeverity.Verbose
                });
            });
        });

        return builder;
    }

    public static IHostBuilder AddFirestore(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddFirestoreClient(firestore =>
            {
                
            });
        });

        return builder;
    }
}
