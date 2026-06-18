using System.Collections.Generic;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class RunTaskCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public RunTaskCommand(string? taskName, List<string> startPoints, string? userName, int newPartsCreateLimit)
    {
        TaskName = taskName;
        StartPoints = startPoints;
        UserName = userName;
        NewPartsCreateLimit = newPartsCreateLimit;
    }

    public string? TaskName { get; }
    public List<string> StartPoints { get; }
    public string? UserName { get; }
    public int NewPartsCreateLimit { get; }
}
