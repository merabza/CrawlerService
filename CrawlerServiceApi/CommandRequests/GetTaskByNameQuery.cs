using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetTaskByNameQuery(string Name) : IQuery<ApiNullableResult<TaskDto>>;