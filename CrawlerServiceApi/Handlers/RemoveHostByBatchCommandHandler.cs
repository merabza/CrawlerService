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

internal sealed class RemoveHostByBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<RemoveHostByBatchCommand, bool>
{
    public Task<OneOf<bool, ErrorOmd[]>> Handle(RemoveHostByBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        if (batch is null)
        {
            return Task.FromResult<OneOf<bool, ErrorOmd[]>>(new[]
            {
                CrawlerServiceErrors.BatchWithNameNotFound(request.BatchName)
            });
        }

        repository.RemoveHostNamesByBatch(batch, request.SchemeName, request.HostName);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, ErrorOmd[]>>(true);
    }
}
