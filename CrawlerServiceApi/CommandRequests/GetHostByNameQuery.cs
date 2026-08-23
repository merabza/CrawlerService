using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostByNameQuery(string Name) : IQueryOmd<ApiNullableResult<HostDto>>;
