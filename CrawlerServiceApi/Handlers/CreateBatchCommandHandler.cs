using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceRoot.Domain.Batches;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class CreateBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateBatchCommand, BatchDto>
{
    public Task<Result<BatchDto>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        Batch created = repository.CreateBatch(request.Batch.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<BatchDto>>(created.ToDto());
    }
}
