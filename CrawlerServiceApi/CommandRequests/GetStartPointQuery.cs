using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetStartPointQuery(int TaskId, string StartPoint) : IQuery<ApiNullableResult<TaskStartPointDto>>;