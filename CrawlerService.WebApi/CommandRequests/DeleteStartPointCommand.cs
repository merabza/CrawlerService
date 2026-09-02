using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record DeleteStartPointCommand(int TaskId, string StartPoint) : ICommand<bool>;
