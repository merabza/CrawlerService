using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetSchemesListQuery : IQuery<List<SchemeDto>>;

public sealed record GetSchemeByNameQuery(string Name) : IQuery<ApiNullableResult<SchemeDto>>;

public sealed record CreateSchemeCommand(SchemeDto Scheme) : ICommand<SchemeDto>;

public sealed record UpdateSchemeCommand(SchemeDto Scheme) : ICommand<bool>;

public sealed record DeleteSchemeCommand(string Name) : ICommand<bool>;
