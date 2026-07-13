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

internal sealed class AddHostByBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<AddHostByBatchCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(AddHostByBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        if (batch is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.BatchWithNameNotFound(request.BatchName)
            });
        }

        repository.AddHostNamesByBatch(batch, request.SchemeName, request.HostName);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}
