using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteBatchCommand(string Name) : ICommandOmd<bool>;
