using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class RunBatchCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public RunBatchCommand(string? batchName, string? userName, int newPartsCreateLimit)
    {
        BatchName = batchName;
        UserName = userName;
        NewPartsCreateLimit = newPartsCreateLimit;
    }

    public string? BatchName { get; }
    public string? UserName { get; }
    public int NewPartsCreateLimit { get; }
}
