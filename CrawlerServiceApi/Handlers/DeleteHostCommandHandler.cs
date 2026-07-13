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

internal sealed class DeleteHostCommandHandler(ICrawlerRepository repository) : ICommandHandler<DeleteHostCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteHostCommand request, CancellationToken cancellationToken)
    {
        HostModel? host = repository.GetHostByName(request.Name);
        if (host is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.HostWithNameNotFound(request.Name)
            });
        }

        repository.DeleteHost(host);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}