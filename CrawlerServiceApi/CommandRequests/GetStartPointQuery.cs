using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetStartPointQuery(int TaskId, string StartPoint) : IQuery<ApiNullableResult<TaskStartPointDto>>;
