namespace Bihyung.Models;

public struct Subscriber
{
    /// <summary>
    ///     The Discord server that owns this channel.
    /// </summary>
    public ulong GuildId { get; init; }

    /// <summary>
    ///     The forum channel containing the thread.
    /// </summary>
    public ulong ChannelId { get; init; }

    /// <summary>
    ///     The thread to post notifications to.
    /// </summary>
    public ulong ThreadId { get; init; }
}
