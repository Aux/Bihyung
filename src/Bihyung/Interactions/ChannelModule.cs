using Bihyung.Models;
using Discord;
using Discord.Interactions;
using LiteDB;

namespace Bihyung.Interactions;

[Group("set-channel", "Set the notification channel for a category")]
public class ChannelModule : InteractionModuleBase<SocketInteractionContext>
{
    private LiteDatabase _db;
    private ILiteCollection<GuildSettings> _collection;
    private GuildSettings _settings;

    public override void BeforeExecute(ICommandInfo command)
    {
        _db = new LiteDatabase(Constants.DbFile);
        _collection = _db.GetCollection<GuildSettings>();
        _settings = _collection.FindOne(x => x.Id == Context.Guild.Id);
        if (_settings == null)
        {
            _settings = new(Context.Guild.Id);
            _collection.Insert(_settings);
        }
    }

    public override void AfterExecute(ICommandInfo command)
    {
        _db.Dispose();
    }

    [SlashCommand("comics", "Set the comic notification channel")]
    public Task ComicsAsync([Autocomplete] IForumChannel channel)
    {
        if (_settings.ComicsChannelId == channel.Id)
            return RespondAsync($"{channel.Mention} is already configured.", ephemeral: true);

        _settings.ComicsChannelId = channel.Id;
        _collection.Update(_settings);

        return RespondAsync($"{channel.Mention} has been set.", ephemeral: true);

    }

    [SlashCommand("anime", "Set the anime notification channel")]
    public Task AnimeAsync([Autocomplete] IForumChannel channel)
    {
        if (_settings.AnimeChannelId == channel.Id)
            return RespondAsync($"{channel.Mention} is already configured.", ephemeral: true);

        _settings.AnimeChannelId = channel.Id;
        _collection.Update(_settings);

        return RespondAsync($"{channel.Mention} has been set.", ephemeral: true);
    }
}
