using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<UpdateBatchCommand, bool>
{
    public Task<OneOf<bool, ErrorOmd[]>> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateBatch(request.Batch.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, ErrorOmd[]>>(true);
    }
}
