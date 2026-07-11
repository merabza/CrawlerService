using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateSchemeCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateSchemeCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateScheme(request.Scheme.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}