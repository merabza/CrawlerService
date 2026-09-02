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

internal sealed class GetTaskByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetTaskByNameQuery, ApiNullableResult<TaskDto>>
{
    public Task<Result<ApiNullableResult<TaskDto>>> Handle(GetTaskByNameQuery request,
        CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        return Task.FromResult<Result<ApiNullableResult<TaskDto>>>(
            new ApiNullableResult<TaskDto> { Value = task?.ToDto() });
    }
}
