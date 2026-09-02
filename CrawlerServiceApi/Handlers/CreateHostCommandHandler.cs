using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceRoot.Domain.HostModels;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class CreateHostCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateHostCommand, HostDto>
{
    public Task<Result<HostDto>> Handle(CreateHostCommand request, CancellationToken cancellationToken)
    {
        HostModel created = repository.CreateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<HostDto>>(created.ToDto());
    }
}
