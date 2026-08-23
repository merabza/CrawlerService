using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record CreateHostCommand(HostDto Host) : ICommandOmd<HostDto>;
