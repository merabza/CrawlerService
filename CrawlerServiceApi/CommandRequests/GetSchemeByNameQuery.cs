using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetSchemeByNameQuery(string Name) : IQuery<ApiNullableResult<SchemeDto>>;