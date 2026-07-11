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

internal sealed class GetTaskByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetTaskByNameQuery, ApiNullableResult<TaskDto>>
{
    public Task<OneOf<ApiNullableResult<TaskDto>, Error[]>> Handle(GetTaskByNameQuery request,
        CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<TaskDto>, Error[]>>(
            new ApiNullableResult<TaskDto> { Value = task?.ToDto() });
    }
}