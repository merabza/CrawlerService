using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.DbModels;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class DeleteSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteSchemeCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteSchemeCommand request, CancellationToken cancellationToken)
    {
        SchemeModel? scheme = repository.GetSchemeByName(request.Name);
        if (scheme is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.SchemeWithNameNotFound(request.Name)
            });
        }

        repository.DeleteScheme(scheme);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}