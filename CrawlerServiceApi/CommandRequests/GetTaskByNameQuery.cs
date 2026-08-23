using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetTaskByNameQuery(string Name) : IQueryOmd<ApiNullableResult<TaskDto>>;
