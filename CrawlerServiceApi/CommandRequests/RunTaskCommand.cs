using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class RunTaskCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public RunTaskCommand(string? taskName, string? userName, int newPartsCreateLimit)
    {
        TaskName = taskName;
        UserName = userName;
        NewPartsCreateLimit = newPartsCreateLimit;
    }

    public string? TaskName { get; }
    public string? UserName { get; }
    public int NewPartsCreateLimit { get; }
}
