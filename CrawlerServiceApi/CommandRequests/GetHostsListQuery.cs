using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostsListQuery : IQuery<List<HostDto>>;
