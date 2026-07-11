using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetHostsListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostsListQuery, List<HostDto>>
{
    public Task<OneOf<List<HostDto>, Error[]>> Handle(GetHostsListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<HostDto>, Error[]>>(repository.GetHostsList().Select(host => host.ToDto())
            .ToList());
    }
}
