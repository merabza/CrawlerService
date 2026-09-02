using System.Collections.Generic;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record GetHostStartUrlNamesByBatchQuery(string BatchName) : IQuery<List<string>>;
