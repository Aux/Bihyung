namespace Bihyung.Models;

public record class WebtoonComic(
    string Title,
    string Author,
    WebtoonUrl Url
    )
{
    public string ThumbnailUrl { get; set; }
    public DayOfWeek? DayOfWeek { get; set; }
}