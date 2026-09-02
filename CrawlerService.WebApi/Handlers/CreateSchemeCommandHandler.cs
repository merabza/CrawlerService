using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceRoot.Domain.SchemeModels;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class CreateSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateSchemeCommand, SchemeDto>
{
    public Task<Result<SchemeDto>> Handle(CreateSchemeCommand request, CancellationToken cancellationToken)
    {
        SchemeModel created = repository.CreateScheme(request.Scheme.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<SchemeDto>>(created.ToDto());
    }
}
