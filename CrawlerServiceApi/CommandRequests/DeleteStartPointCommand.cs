using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record DeleteStartPointCommand(int TaskId, string StartPoint) : ICommand<bool>;
