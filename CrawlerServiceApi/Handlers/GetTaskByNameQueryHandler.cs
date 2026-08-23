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

internal sealed class GetTaskByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandlerOmd<GetTaskByNameQuery, ApiNullableResult<TaskDto>>
{
    public Task<OneOf<ApiNullableResult<TaskDto>, ErrorOmd[]>> Handle(GetTaskByNameQuery request,
        CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<TaskDto>, ErrorOmd[]>>(
            new ApiNullableResult<TaskDto> { Value = task?.ToDto() });
    }
}
