using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class DeleteBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<DeleteBatchCommand, bool>
{
    public Task<OneOf<bool, ErrorOmd[]>> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.Name);
        if (batch is null)
        {
            return Task.FromResult<OneOf<bool, ErrorOmd[]>>(new[]
            {
                CrawlerServiceErrors.BatchWithNameNotFound(request.Name)
            });
        }

        repository.DeleteBatch(batch);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, ErrorOmd[]>>(true);
    }
}
