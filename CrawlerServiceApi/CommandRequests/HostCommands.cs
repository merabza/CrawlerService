using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostsListQuery : IQuery<List<HostDto>>;

public sealed record GetHostByNameQuery(string Name) : IQuery<ApiNullableResult<HostDto>>;

public sealed record CreateHostCommand(HostDto Host) : ICommand<HostDto>;

public sealed record UpdateHostCommand(HostDto Host) : ICommand<bool>;

public sealed record DeleteHostCommand(string Name) : ICommand<bool>;
