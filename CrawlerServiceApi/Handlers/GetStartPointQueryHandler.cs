using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetStartPointQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetStartPointQuery, ApiNullableResult<TaskStartPointDto>>
{
    public Task<Result<ApiNullableResult<TaskStartPointDto>>> Handle(GetStartPointQuery request,
        CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        return Task.FromResult<Result<ApiNullableResult<TaskStartPointDto>>>(
            new ApiNullableResult<TaskStartPointDto> { Value = startPoint?.ToDto() });
    }
}
