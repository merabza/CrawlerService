using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceRoot.Domain.Batches;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class RemoveHostByBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<RemoveHostByBatchCommand, bool>
{
    public Task<Result<bool>> Handle(RemoveHostByBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        if (batch is null)
        {
            return Task.FromResult<Result<bool>>(CrawlerServiceErrors.BatchWithNameNotFound(request.BatchName));
        }

        repository.RemoveHostNamesByBatch(batch, request.SchemeName, request.HostName);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
