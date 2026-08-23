using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetHostStartUrlNamesByBatchQueryHandler(ICrawlerRepository repository)
    : IQueryHandlerOmd<GetHostStartUrlNamesByBatchQuery, List<string>>
{
    public Task<OneOf<List<string>, ErrorOmd[]>> Handle(GetHostStartUrlNamesByBatchQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        return Task.FromResult<OneOf<List<string>, ErrorOmd[]>>(batch is null
            ? []
            : repository.GetHostStartUrlNamesByBatch(batch));
    }
}
