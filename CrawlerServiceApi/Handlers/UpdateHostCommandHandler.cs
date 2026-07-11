using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateHostCommandHandler(ICrawlerRepository repository) : ICommandHandler<UpdateHostCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateHostCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}