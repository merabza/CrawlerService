using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceRoot.Domain.SchemeModels;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class DeleteSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteSchemeCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteSchemeCommand request, CancellationToken cancellationToken)
    {
        SchemeModel? scheme = repository.GetSchemeByName(request.Name);
        if (scheme is null)
        {
            return Task.FromResult<Result<bool>>(CrawlerServiceErrors.SchemeWithNameNotFound(request.Name));
        }

        repository.DeleteScheme(scheme);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
