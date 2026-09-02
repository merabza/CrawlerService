using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteHostCommand(string Name) : ICommand<bool>;
