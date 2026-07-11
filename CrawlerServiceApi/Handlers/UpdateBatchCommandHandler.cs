using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateBatchCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateBatch(request.Batch.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}