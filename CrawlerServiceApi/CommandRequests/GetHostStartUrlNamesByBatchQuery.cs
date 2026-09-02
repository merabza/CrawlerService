using System.Collections.Generic;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetHostStartUrlNamesByBatchQuery(string BatchName) : IQuery<List<string>>;
