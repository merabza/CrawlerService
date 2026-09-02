using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class GetBatchesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetBatchesListQuery, List<BatchDto>>
{
    public Task<Result<List<BatchDto>>> Handle(GetBatchesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<BatchDto>>>(repository.GetBatchesList().Select(batch => batch.ToDto())
            .ToList());
    }
}
