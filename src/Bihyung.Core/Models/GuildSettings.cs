namespace Bihyung.Models;

public record class GuildSettings(ulong Id)
{
    public ulong? ComicsChannelId { get; set; }
    public ulong? AnimeChannelId { get; set; }
}