namespace Bihyung.Models;

public class WebtoonComic : Webcomic
{
    public string Language { get; init; }
    public string Genre { get; init; }
    public string Slug { get; set; }
    public string UpdateRate { get; set; }
}
