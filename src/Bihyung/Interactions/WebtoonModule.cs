using Bihyung.Models;
using Bihyung.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using LiteDB;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace Bihyung.Interactions;

public class WebtoonModule(WebtoonService _webtoon) 
    : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("comic-info", "Get information about a configured webcomic")]
    public Task InfoAsync([Autocomplete] WebtoonComic comic)
    {
        // TypeReader pulls Webcomic from db
        // Get total polls amount
        // Get last poll timestamp
        // Link to notification thread

        return Task.CompletedTask;
    }

    [AutocompleteCommand("comic", "comic-info")]
    public Task InfoAutocompleteAsync()
        => WebcomicAutocompleteAsync();

    [SlashCommand("force-update", "Force an update check for a specific webcomic")]
    public Task ForceAsync()
    {
        return Task.CompletedTask;
    }

    [SlashCommand("add-comic", "Add a webcomic that the bot has already downloaded.")]
    public async Task AddAsync([Autocomplete] WebtoonComic comic)
    {
        using var db = new LiteDatabase(Constants.DbFile);
        var settings = db.GetCollection<GuildSettings>().FindOne(x => x.Id == Context.Guild.Id);
        if (settings?.ComicsChannelId == null)      // No notification channel set
        {
            await RespondAsync($"You must set a notification channel first with `/set-channel comic #channel-name`", ephemeral: true);
            return;
        }

        var threads = db.GetCollection<WebtoonThread>();
        var webtoonThread = threads.FindOne(x => x.WebtoonId == comic.Url.Id);
        if (webtoonThread != null)                  // Duplicate thread
        {
            var existingThread = (await Context.Client.GetChannelAsync(webtoonThread.ThreadId)) as IThreadChannel;
            await RespondAsync($"There is already a thread for this comic at {existingThread.Mention}", ephemeral: true);
            return;
        }

        var forum = (await Context.Client.GetChannelAsync(settings.ComicsChannelId.Value)) as IForumChannel;

        using var rss = XmlReader.Create(comic.Url.GetComicRssUrl());
        var feed = SyndicationFeed.Load(rss);

        string opText = $"[**{comic.Title}** by *{comic.Author}*]({comic.Url.GetComicPageUrl()})\n\n" +
            $"Updates on **{comic.DayOfWeek}s**\n\n{feed.Description.Text}";

        var thread = await forum.CreatePostAsync(comic.Title, text: opText);

        var latest = feed.Items.FirstOrDefault();
        if (latest != null)
        {
            var unixTime = latest.PublishDate.ToUnixTimeSeconds();
            string postText = $"[{latest.Title.Text}]({latest.Links.FirstOrDefault().Uri}) was posted <t:{unixTime}:R>";

            var post = thread.SendMessageAsync(postText);
        }

        webtoonThread = new(Context.Guild.Id, thread.Id, comic.Url.Id);
        threads.Insert(webtoonThread);

        await RespondAsync($"Done, you can see the new thread here {thread.Mention}", ephemeral: true);
    }

    [AutocompleteCommand("comic", "add-comic")]
    public Task AddAutocompleteAsync()
        => WebcomicAutocompleteAsync();
    private async Task WebcomicAutocompleteAsync()
    {
        string userInput = (Context.Interaction as SocketAutocompleteInteraction).Data.Current.Value.ToString();

        var results = new List<AutocompleteResult>();
        using (var db = new LiteDatabase(Constants.DbFile))
        {
            var matches = db.GetCollection<WebtoonComic>().Query()
                .Where(x => x.Title.Contains(userInput, StringComparison.InvariantCultureIgnoreCase));

            foreach (var match in matches.ToList().Take(25))
                results.Add(new AutocompleteResult(match.Title, match.Url.Id));
        }

        await (Context.Interaction as SocketAutocompleteInteraction).RespondAsync(results);
    }

    [SlashCommand("remove-comic", "Remove an existing webcomic notifier")]
    public Task RemoveAsync()
    {
        // Confirm user selection is correct
        // Remove notification subscribers, not the webcomic's base model.

        return Task.CompletedTask;
    }

    [SlashCommand("find-comic", "Find a new webcomic if not already downloaded.")]
    public async Task FindAsync(string query)
    {
        var matches = await _webtoon.SearchAsync(query);
        if (matches?.Count() < 1)   // No results
        {
            await RespondAsync($"I couldn't find any webcomics matching `{query}`", ephemeral: true);
            return;
        }

        string msg;
        var components = new ComponentBuilder();
        if (matches?.Count() == 1)  // Single result yes/no buttons
        {
            var match = matches.First();
            msg = $"Do you want to add [**{match.Title}** by *{match.Author}*]({match.Url.GetComicPageUrl()})";
            components.WithButton("Yes", "select-webtoon-yes:" + match.Url.Id, ButtonStyle.Success)
                .WithButton("No", "select-webtoon-no", ButtonStyle.Danger);
        } 
        else                      // Multiple result select menu
        {
            msg = $"I found {matches.Count()} comics matching `{query}`, which one would you like to add?";
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Select a matching webcomic")
                .WithCustomId("search-webtoon-menu")
                .WithMinValues(1)
                .WithMaxValues(1);

            foreach (var match in matches)
                menuBuilder.AddOption(match.Title, match.Url.Id, match.Author);

            components.WithSelectMenu(menuBuilder);
        }

        await RespondAsync(msg, components: components.Build(), ephemeral: true);
    }

    [ComponentInteraction("search-webtoon-menu")]
    public Task SearchWebtoonMenuAsync(string id)
        => SelectWebtoonYesAsync(id);
    [ComponentInteraction("select-webtoon-yes:*")]
    public async Task SelectWebtoonYesAsync(string id)
    {
        WebtoonComic comic;
        using (var db = new LiteDatabase(Constants.DbFile))
        {
            comic = db.GetCollection<WebtoonComic>().FindOne(x => x.Url.Id == id);
            await _webtoon.GetDetailsAsync(comic);
        }
        await AddAsync(comic);
    }

    [ComponentInteraction("select-webtoon-no")]
    public async Task SelectWebtoonNoAsync()
    {
        await RespondAsync($"Ok, try the command again later with a different search term.", ephemeral: true);
    }
}
