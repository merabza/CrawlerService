using System.Collections.Generic;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class TestOnePageCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public TestOnePageCommand(string? taskName, string? strUrName, List<string> startPoints, string? userName,
        bool deleteContentForReanalyze, int newPartsCreateLimit)
    {
        TaskName = taskName;
        Url = strUrName;
        StartPoints = startPoints;
        UserName = userName;
        DeleteContentForReanalyze = deleteContentForReanalyze;
        NewPartsCreateLimit = newPartsCreateLimit;
    }

    public string? TaskName { get; }
    public string? Url { get; }
    public List<string> StartPoints { get; }
    public string? UserName { get; }
    public bool DeleteContentForReanalyze { get; }
    public int NewPartsCreateLimit { get; }
}
