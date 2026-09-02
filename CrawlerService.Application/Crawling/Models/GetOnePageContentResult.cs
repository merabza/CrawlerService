using System;
using System.Net;

namespace CrawlerService.Application.Crawling.Models;

public sealed class GetOnePageContentResult
{
    public HttpStatusCode StatusCode { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Content { get; set; }
    public string? Location { get; set; }
}
