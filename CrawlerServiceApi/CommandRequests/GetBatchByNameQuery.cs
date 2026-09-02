using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetBatchByNameQuery(string Name) : IQuery<ApiNullableResult<BatchDto>>;
