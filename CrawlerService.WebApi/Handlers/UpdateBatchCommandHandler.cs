using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

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
