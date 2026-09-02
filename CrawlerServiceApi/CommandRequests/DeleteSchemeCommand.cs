using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteSchemeCommand(string Name) : ICommand<bool>;
