using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteSchemeCommand(string Name) : ICommand<bool>;