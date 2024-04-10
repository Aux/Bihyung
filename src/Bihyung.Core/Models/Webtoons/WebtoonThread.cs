namespace Bihyung.Models;

public record class WebtoonThread(
    ulong GuildId,
    ulong ThreadId,
    string WebtoonId
    );