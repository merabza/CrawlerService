using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class GetHostsListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostsListQuery, List<HostDto>>
{
    public Task<Result<List<HostDto>>> Handle(GetHostsListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<HostDto>>>(repository.GetHostsList().Select(host => host.ToDto()).ToList());
    }
}
