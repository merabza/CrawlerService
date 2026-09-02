using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record DeleteBatchCommand(string Name) : ICommand<bool>;
