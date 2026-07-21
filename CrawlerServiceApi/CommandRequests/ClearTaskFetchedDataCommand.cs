using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record ClearTaskFetchedDataCommand(string Name) : ICommand<bool>;