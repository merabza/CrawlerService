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

internal sealed class GetBatchesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetBatchesListQuery, List<BatchDto>>
{
    public Task<OneOf<List<BatchDto>, Error[]>> Handle(GetBatchesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<BatchDto>, Error[]>>(repository.GetBatchesList()
            .Select(batch => batch.ToDto()).ToList());
    }
}
