using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record ClearTaskFetchedDataCommand(string Name) : ICommand<bool>;
