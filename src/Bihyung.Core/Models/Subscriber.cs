namespace Bihyung.Models;

public struct Subscriber
{
    /// <summary>
    ///     The discord server's id.
    /// </summary>
    public ulong GuildId { get; init; }

    /// <summary>
    ///     The forum channel the bot manages notifications in.
    /// </summary>
    public ulong ChannelId { get; set; }

    /// <summary>
    ///     The thread to post notifications to.
    /// </summary>
    public ulong ThreadId { get; init; }

    /// <summary>
    ///     The webcomic being subscribed to.
    /// </summary>
    public string WebcomicId { get; init; }
}
