using CrawlerServiceRoot.Domain.Terms;

namespace CrawlerService.Application.Crawling.Models;

public sealed class UriTerm
{
    public UriTerm(ETermType termType)
    {
        TermType = termType;
    }

    public UriTerm(ETermType termType, string context)
    {
        TermType = termType;
        Context = context.Truncate(TermConstants.TermTextLength);
    }

    public ETermType TermType { get; set; }

    public string? Context { get; set; }
}
