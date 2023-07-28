namespace Bihyung;

public static class Webtoons
{
    public const string BaseUrl = "https://www.webtoons.com/";

    public const string PageFormat = "{0}/{1}/{2}/list?title={3}";
    public const string RssFormat = "{0}/{1}/{2}/rss?title={3}";
    public const string SearchFormat = "{0}/search?keyword={1}";

    public const string ComicGenreIdSelector = "";
    public const string ComicTitleSelector = "";
    public const string ComicAuthorSelector = "";
    public const string ComicFrequencySelector = "";

    public const string SearchOriginalsListSelector = "";
    public const string SearchOriginalsTitleSelector = "";
    public const string SearchOriginalsAuthorSelector = "";
    public const string SearchOriginalsThumbnailSelector = "";
    public const string SearchOriginalsUrlSelector = "";

    public const string SearchCanvasListSelector = "";
    public const string SearchCanvasTitleSelector = "";
    public const string SearchCanvasAuthorSelector = "";
    public const string SearchCanvasThumbnailSelector = "";
    public const string SearchCanvasUrlSelector = "";

    public static string GetComicPageUrl(string language, string genre, string slug, string id)
        => BaseUrl + string.Format(PageFormat, language, genre, slug, id);
    public static string GetComicRssUrl(string language, string genre, string slug, string id)
        => BaseUrl + string.Format(RssFormat, language, genre, slug, id);

    public static string GetSearchUrl(string language, string keyword)
        => BaseUrl + string.Format(SearchFormat, language, Uri.EscapeDataString(keyword));
}
