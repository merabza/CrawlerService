using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record RemoveHostByBatchCommand(string BatchName, string SchemeName, string HostName) : ICommand<bool>;
