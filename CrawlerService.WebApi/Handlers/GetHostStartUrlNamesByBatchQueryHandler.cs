using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.Batches;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class GetHostStartUrlNamesByBatchQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostStartUrlNamesByBatchQuery, List<string>>
{
    public Task<Result<List<string>>> Handle(GetHostStartUrlNamesByBatchQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        List<string> startUrlNames = batch is null ? [] : repository.GetHostStartUrlNamesByBatch(batch);
        return Task.FromResult<Result<List<string>>>(startUrlNames);
    }
}
