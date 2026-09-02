using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.Batches;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class AddHostByBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<AddHostByBatchCommand, bool>
{
    public Task<Result<bool>> Handle(AddHostByBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        if (batch is null)
        {
            return Task.FromResult<Result<bool>>(CrawlerServiceErrors.BatchWithNameNotFound(request.BatchName));
        }

        repository.AddHostNamesByBatch(batch, request.SchemeName, request.HostName);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
