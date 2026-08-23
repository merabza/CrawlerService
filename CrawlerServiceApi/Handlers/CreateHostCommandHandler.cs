using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class CreateHostCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<CreateHostCommand, HostDto>
{
    public Task<OneOf<HostDto, ErrorOmd[]>> Handle(CreateHostCommand request, CancellationToken cancellationToken)
    {
        HostModel created = repository.CreateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<HostDto, ErrorOmd[]>>(created.ToDto());
    }
}
