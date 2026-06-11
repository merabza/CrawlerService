using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class RunBatchCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public RunBatchCommand(string? batchName, string? userName)
    {
        BatchName = batchName;
        UserName = userName;
    }

    public string? BatchName { get; }
    public string? UserName { get; }
}
