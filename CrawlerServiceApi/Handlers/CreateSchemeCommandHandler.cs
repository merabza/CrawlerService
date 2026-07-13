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

internal sealed class CreateSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateSchemeCommand, SchemeDto>
{
    public Task<OneOf<SchemeDto, Error[]>> Handle(CreateSchemeCommand request, CancellationToken cancellationToken)
    {
        SchemeModel created = repository.CreateScheme(request.Scheme.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<SchemeDto, Error[]>>(created.ToDto());
    }
}
