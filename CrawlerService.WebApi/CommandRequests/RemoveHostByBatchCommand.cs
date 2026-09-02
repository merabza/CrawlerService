using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record RemoveHostByBatchCommand(string BatchName, string SchemeName, string HostName) : ICommand<bool>;
