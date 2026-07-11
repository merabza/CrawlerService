using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteHostCommand(string Name) : ICommand<bool>;