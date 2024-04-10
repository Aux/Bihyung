using Bihyung.Models;
using Discord;
using Discord.Interactions;
using LiteDB;

namespace Bihyung.TypeReaders;

public class WebtoonComicConverter : TypeConverter<WebtoonComic>
{
    public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        => WebtoonTypeReader.ReadAsync(option.Value?.ToString());
}

public class WebtoonComicComponentConverter : ComponentTypeConverter<WebtoonComic>
{
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IComponentInteractionData option, IServiceProvider services)
        => WebtoonTypeReader.ReadAsync(option.Value?.ToString());
}

public class WebtoonTypeReader : TypeReader<WebtoonComic>
{
    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option, IServiceProvider services)
        => ReadAsync(option);

    public static Task<TypeConverterResult> ReadAsync(string input)
    {
        using (var db = new LiteDatabase(Constants.DbFile))
        {
            if (input == null)
                return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.BadArgs, ""));

            var queryable = db.GetCollection<WebtoonComic>().Query();
            if (int.TryParse(input, out int inputId))
            {
                var matches = queryable.Where(x => x.Url.Id == inputId.ToString());
                if (matches?.Count() > 0)
                    return Task.FromResult(TypeConverterResult.FromSuccess(matches.First()));
            } else
            {
                var matches = queryable.Where(x => x.Title == input);
                if (matches?.Count() > 0)
                    return Task.FromResult(TypeConverterResult.FromSuccess(matches.First()));
            }
        }

        return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.BadArgs, $"Couldn't find a comic like `{input}`"));
    }
}
