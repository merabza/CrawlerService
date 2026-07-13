using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetTasksListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetTasksListQuery, List<TaskDto>>
{
    public Task<OneOf<List<TaskDto>, Error[]>> Handle(GetTasksListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<TaskDto>, Error[]>>(repository.GetTasksList().Select(task => task.ToDto())
            .ToList());
    }
}
