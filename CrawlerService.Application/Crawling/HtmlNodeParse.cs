using HtmlAgilityPack;

namespace CrawlerService.Application.Crawling;

public sealed class HtmlNodeParse
{
    public HtmlNodeParse(HtmlNode htmlNode, bool inScript)
    {
        HtmlNode = htmlNode;
        InScript = inScript;
    }

    public HtmlNode HtmlNode { get; }
    public bool InScript { get; }
}
