using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record GetTaskByNameQuery(string Name) : IQuery<ApiNullableResult<TaskDto>>;
