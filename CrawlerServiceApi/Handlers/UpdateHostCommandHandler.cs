using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateHostCommandHandler(ICrawlerRepository repository) : ICommandHandler<UpdateHostCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateHostCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
