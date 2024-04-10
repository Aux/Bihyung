namespace Bihyung.Models;

public record struct WebtoonUrl(
    string Language,
    string Genre,
    string Title,
    string Id
    )
{
    public static WebtoonUrl Parse(string url) 
        => Webtoon.DeconstructUrl(url);

    public string GetComicPageUrl()
        => Webtoon.BaseUrl + string.Format(Webtoon.PageFormat, Language, Genre, Title, Id);
    public string GetComicRssUrl()
        => Webtoon.BaseUrl + string.Format(Webtoon.RssFormat, Language, Genre, Title, Id);

    public override string ToString() 
        => GetComicRssUrl();
}
