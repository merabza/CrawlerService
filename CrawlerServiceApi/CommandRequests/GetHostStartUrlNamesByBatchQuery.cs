using System.Collections.Generic;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostStartUrlNamesByBatchQuery(string BatchName) : IQuery<List<string>>;