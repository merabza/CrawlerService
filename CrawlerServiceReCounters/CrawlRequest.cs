using System.Collections.Generic;

namespace CrawlerServiceReCounters;

public sealed class CrawlRequest
{
    public ECrawlKind Kind { get; init; }
    public string? UserName { get; init; }
    public string? BatchName { get; init; }
    public string? TaskName { get; init; }
    public string? Url { get; init; }
    public List<string> StartPoints { get; init; } = [];
    public bool DeleteContentForReanalyze { get; init; }
    public int NewPartsCreateLimit { get; init; }
}
