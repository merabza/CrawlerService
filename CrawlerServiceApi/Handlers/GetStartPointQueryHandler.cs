using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetStartPointQueryHandler(ICrawlerRepository repository)
    : IQueryHandlerOmd<GetStartPointQuery, ApiNullableResult<TaskStartPointDto>>
{
    public Task<OneOf<ApiNullableResult<TaskStartPointDto>, ErrorOmd[]>> Handle(GetStartPointQuery request,
        CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        return Task.FromResult<OneOf<ApiNullableResult<TaskStartPointDto>, ErrorOmd[]>>(
            new ApiNullableResult<TaskStartPointDto> { Value = startPoint?.ToDto() });
    }
}
