using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class UpdateHostCommandHandler(ICrawlerRepository repository) : ICommandHandler<UpdateHostCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateHostCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
