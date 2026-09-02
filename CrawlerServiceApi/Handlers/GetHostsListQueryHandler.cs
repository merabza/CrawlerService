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

internal sealed class GetHostsListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostsListQuery, List<HostDto>>
{
    public Task<Result<List<HostDto>>> Handle(GetHostsListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<HostDto>>>(repository.GetHostsList().Select(host => host.ToDto()).ToList());
    }
}
