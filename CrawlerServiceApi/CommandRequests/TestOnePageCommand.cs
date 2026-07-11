using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed class TestOnePageCommand : ICommand<bool>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public TestOnePageCommand(string? taskName, string? strUrName, string? userName, bool deleteContentForReanalyze,
        int newPartsCreateLimit)
    {
        TaskName = taskName;
        Url = strUrName;
        UserName = userName;
        DeleteContentForReanalyze = deleteContentForReanalyze;
        NewPartsCreateLimit = newPartsCreateLimit;
    }

    public string? TaskName { get; }
    public string? Url { get; }
    public string? UserName { get; }
    public bool DeleteContentForReanalyze { get; }
    public int NewPartsCreateLimit { get; }
}
