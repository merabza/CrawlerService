using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceRoot.Domain.TaskStartPoints;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

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
