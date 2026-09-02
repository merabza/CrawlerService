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

internal sealed class GetBatchesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetBatchesListQuery, List<BatchDto>>
{
    public Task<Result<List<BatchDto>>> Handle(GetBatchesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<BatchDto>>>(repository.GetBatchesList().Select(batch => batch.ToDto())
            .ToList());
    }
}
