using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteTaskCommand(string Name) : ICommandOmd<bool>;
