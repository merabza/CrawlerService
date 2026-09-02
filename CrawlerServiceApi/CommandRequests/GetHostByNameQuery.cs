using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostByNameQuery(string Name) : IQuery<ApiNullableResult<HostDto>>;
