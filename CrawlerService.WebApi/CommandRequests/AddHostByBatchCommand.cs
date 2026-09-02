using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record AddHostByBatchCommand(string BatchName, string SchemeName, string HostName) : ICommand<bool>;
