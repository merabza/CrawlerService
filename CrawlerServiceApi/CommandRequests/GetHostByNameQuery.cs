using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostByNameQuery(string Name) : IQuery<ApiNullableResult<HostDto>>;