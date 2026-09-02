using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record DeleteTaskCommand(string Name) : ICommand<bool>;
