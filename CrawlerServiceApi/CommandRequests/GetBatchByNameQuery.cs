using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetBatchByNameQuery(string Name) : IQuery<ApiNullableResult<BatchDto>>;