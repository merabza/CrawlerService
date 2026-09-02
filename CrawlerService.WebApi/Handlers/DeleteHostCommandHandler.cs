using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.HostModels;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class DeleteHostCommandHandler(ICrawlerRepository repository) : ICommandHandler<DeleteHostCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteHostCommand request, CancellationToken cancellationToken)
    {
        HostModel? host = repository.GetHostByName(request.Name);
        if (host is null)
        {
            return Task.FromResult<Result<bool>>(CrawlerServiceErrors.HostWithNameNotFound(request.Name));
        }

        repository.DeleteHost(host);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
