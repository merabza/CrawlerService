using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateHostCommand(HostDto Host) : ICommand<bool>;
