using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record DeleteHostCommand(string Name) : ICommand<bool>;
