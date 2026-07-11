using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.DbModels;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetHostStartUrlNamesByBatchQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostStartUrlNamesByBatchQuery, List<string>>
{
    public Task<OneOf<List<string>, Error[]>> Handle(GetHostStartUrlNamesByBatchQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        return Task.FromResult<OneOf<List<string>, Error[]>>(batch is null
            ? new List<string>()
            : repository.GetHostStartUrlNamesByBatch(batch));
    }
}