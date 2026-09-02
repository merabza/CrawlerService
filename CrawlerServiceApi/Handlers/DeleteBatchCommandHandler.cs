using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceRoot.Domain.Batches;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class DeleteBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteBatchCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.Name);
        if (batch is null)
        {
            return Task.FromResult<Result<bool>>(CrawlerServiceErrors.BatchWithNameNotFound(request.Name));
        }

        repository.DeleteBatch(batch);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
