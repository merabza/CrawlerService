// ReSharper disable CollectionNeverUpdated.Global

using System;
using System.Collections.Generic;

namespace CrawlerDbModels;

public sealed class UrlModel
{
    public int UrlId { get; set; }
    public required string UrlName { get; set; }
    public int HostId { get; set; }
    public int ExtensionId { get; set; }
    public int SchemeId { get; set; }
    public int UrlHashCode { get; set; }
    public bool IsSiteMap { get; set; }

    public bool IsAllowed { get; set; }

    public HostModel HostNavigation
    {
        get => field ?? throw new InvalidOperationException("Uninitialized property: " + nameof(HostNavigation));
        init;
    }

    public ExtensionModel ExtensionNavigation
    {
        get => field ?? throw new InvalidOperationException("Uninitialized property: " + nameof(ExtensionNavigation));
        init;
    }

    public SchemeModel SchemeNavigation
    {
        get => field ?? throw new InvalidOperationException("Uninitialized property: " + nameof(SchemeNavigation));
        init;
    }

    public ICollection<UrlGraphNode> UrlGraphNodesFrom { get; init; } = new HashSet<UrlGraphNode>();
    public ICollection<UrlGraphNode> UrlGraphNodesGot { get; init; } = new HashSet<UrlGraphNode>();
    public ICollection<TermByUrl> TermsByUrls { get; init; } = new HashSet<TermByUrl>();
    public ICollection<ContentAnalysis> ContentsAnalysis { get; init; } = new HashSet<ContentAnalysis>();
}
