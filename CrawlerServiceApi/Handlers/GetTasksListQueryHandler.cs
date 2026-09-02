using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetTasksListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetTasksListQuery, List<TaskDto>>
{
    public Task<Result<List<TaskDto>>> Handle(GetTasksListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<TaskDto>>>(repository.GetTasksList().Select(task => task.ToDto()).ToList());
    }
}
