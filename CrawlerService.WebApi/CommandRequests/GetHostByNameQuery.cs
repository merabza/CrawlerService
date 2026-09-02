using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record GetHostByNameQuery(string Name) : IQuery<ApiNullableResult<HostDto>>;
