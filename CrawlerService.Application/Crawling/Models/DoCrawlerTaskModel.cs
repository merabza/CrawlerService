using System.Collections.Generic;

namespace CrawlerService.Application.Crawling.Models;

public sealed class DoCrawlerTaskModel
{
    public List<string> StartPoints { get; set; } = [];
}
