using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteBatchCommand(string Name) : ICommand<bool>;
