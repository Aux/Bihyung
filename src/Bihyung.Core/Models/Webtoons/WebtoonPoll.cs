namespace Bihyung.Models;

public record class WebtoonPoll(
    DateTime Timestamp,
    DateTime NextPollAt,
    string ComicId,
    PollResult Result
    );

public enum PollResult
{
    Ok,
    NewChapter,
    Error
}