using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceRoot.Domain.TaskModels;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

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
