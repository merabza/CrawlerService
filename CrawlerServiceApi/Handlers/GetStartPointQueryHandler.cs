using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.DbModels;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetStartPointQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetStartPointQuery, ApiNullableResult<TaskStartPointDto>>
{
    public Task<OneOf<ApiNullableResult<TaskStartPointDto>, Error[]>> Handle(GetStartPointQuery request,
        CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        return Task.FromResult<OneOf<ApiNullableResult<TaskStartPointDto>, Error[]>>(
            new ApiNullableResult<TaskStartPointDto> { Value = startPoint?.ToDto() });
    }
}