using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetSchemesListQuery : IQuery<List<SchemeDto>>;
