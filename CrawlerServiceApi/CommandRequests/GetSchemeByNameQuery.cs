using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetSchemeByNameQuery(string Name) : IQuery<ApiNullableResult<SchemeDto>>;
