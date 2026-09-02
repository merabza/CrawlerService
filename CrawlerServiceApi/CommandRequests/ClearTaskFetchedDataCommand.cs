using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record ClearTaskFetchedDataCommand(string Name) : ICommand<bool>;
