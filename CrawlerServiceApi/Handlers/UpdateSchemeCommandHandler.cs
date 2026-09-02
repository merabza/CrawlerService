using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateSchemeCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateSchemeCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateScheme(request.Scheme.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
