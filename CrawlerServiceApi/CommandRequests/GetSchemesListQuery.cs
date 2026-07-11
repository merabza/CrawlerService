using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetSchemesListQuery : IQuery<List<SchemeDto>>;