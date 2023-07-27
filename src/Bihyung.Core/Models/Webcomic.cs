using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bihyung.Models;

public enum WebcomicSource
{
    Misc = -1,

    Webtoons,
    Tapas,
    Asurascans,
    Flamescans
}

public class Webcomic
{
    public object Id { get; init; }
    public string Name { get; init; }
    public string Author { get; init; }

    public string CoverImageUrl { get; set; }
    public string LinkUrl { get; set; }
    public string PollingUrl { get; set; }

    public WebcomicSource Source { get; init; } 
}
