using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record GetSchemeByNameQuery(string Name) : IQuery<ApiNullableResult<SchemeDto>>;
