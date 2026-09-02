using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateBatchCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateBatch(request.Batch.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
