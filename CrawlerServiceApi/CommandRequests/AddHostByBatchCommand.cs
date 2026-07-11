using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record AddHostByBatchCommand(string BatchName, string SchemeName, string HostName) : ICommand<bool>;