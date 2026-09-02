using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteTaskCommand(string Name) : ICommand<bool>;
