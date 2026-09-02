using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record GetBatchByNameQuery(string Name) : IQuery<ApiNullableResult<BatchDto>>;
