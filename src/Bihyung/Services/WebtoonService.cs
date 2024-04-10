using Bihyung.Models;
using LiteDB;
using Microsoft.Extensions.Logging;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using ZLogger;

namespace Bihyung.Services;

public class WebtoonService
{
    private readonly ILogger<WebtoonService> _logger;
    private readonly ScrapingBrowser _browser;

    public WebtoonService(ILogger<WebtoonService> logger)
    {
        _logger = logger;
        _browser = new ScrapingBrowser();
    }

    public async Task<IEnumerable<WebtoonComic>> SearchAsync(string query)
    {
        var urlSafeQuery = Uri.EscapeDataString(query);
        var queryUri = new Uri(Webtoon.GetSearchUrl("en", urlSafeQuery));

        _logger.ZLogInformation($"Downloading page `{queryUri}`");
        var resultPage = await _browser.NavigateToPageAsync(queryUri);
        var cardList = resultPage.Html.CssSelect(".card_lst > li");

        _logger.ZLogDebug($"Parsing search result cards");
        var results = new List<WebtoonComic>();
        foreach (var listItem in cardList)
        {
            var card = listItem.SelectSingleNode("a");

            var url = card.GetAttributeValue("href");
            var title = card.SelectSingleNode("div/p[@class='subj']").InnerText;
            var author = card.SelectSingleNode("div/p[@class='author']").InnerText;

            results.Add(new(title, author, WebtoonUrl.Parse(url)));
        }

        using (var db = new LiteDatabase(Constants.DbFile))
        {
            var collection = db.GetCollection<WebtoonComic>();
            var newlyFound = results.Where(x => collection.Count(c => c.Url.Id == x.Url.Id) == 0);

            if (newlyFound.Any())
            {
                collection.InsertBulk(newlyFound);
                collection.EnsureIndex(x => x.Url.Id, true);
                _logger.ZLogDebug($"Adding {results.Count} newly found comics to the db");
            }
        }

        _logger.ZLogDebug($"Returning {results.Count} card(s)");
        return results;
    }

    public async Task<WebtoonComic> GetDetailsAsync(WebtoonComic comic)
    {
        var queryUri = new Uri(comic.Url.GetComicPageUrl());
        _logger.ZLogInformation($"Downloading page `{queryUri}`");
        var comicPage = await _browser.NavigateToPageAsync(queryUri);

        var thumbnailUrl = comicPage.Html.SelectSingleNode("/html/head/meta[9]").GetAttributeValue("content");
        comic.ThumbnailUrl = thumbnailUrl;

        var detailsPanel = comicPage.Html.CssSelect("#_asideDetail").Single();
        var frequencyText = detailsPanel.SelectSingleNode("p[@class='day_info']").InnerText.Replace("UPEVERY ", "");

        comic.DayOfWeek = frequencyText switch
        {
            "MONDAY" => DayOfWeek.Monday,
            "TUESDAY" => DayOfWeek.Tuesday,
            "WEDNESDAY" => DayOfWeek.Wednesday,
            "THURSDAY" => DayOfWeek.Thursday,
            "FRIDAY" => DayOfWeek.Friday,
            "SATURDAY" => DayOfWeek.Saturday,
            "SUNDAY" => DayOfWeek.Sunday,
            _ => null,
        };

        return comic;
    }
}