using System.Collections.Generic;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class RunTaskCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public RunTaskCommand(string? taskName, List<string> startPoints, string? userName)
    {
        TaskName = taskName;
        StartPoints = startPoints;
        UserName = userName;
    }

    public string? TaskName { get; }
    public List<string> StartPoints { get; }
    public string? UserName { get; }
}
