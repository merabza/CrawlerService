using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetTaskByNameQuery(string Name) : IQuery<ApiNullableResult<TaskDto>>;
