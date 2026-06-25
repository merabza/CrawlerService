using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetHostsListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostsListQuery, List<HostDto>>
{
    public Task<OneOf<List<HostDto>, Error[]>> Handle(GetHostsListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<HostDto>, Error[]>>(
            repository.GetHostsList().Select(host => host.ToDto()).ToList());
    }
}

internal sealed class GetHostByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostByNameQuery, ApiNullableResult<HostDto>>
{
    public Task<OneOf<ApiNullableResult<HostDto>, Error[]>> Handle(GetHostByNameQuery request,
        CancellationToken cancellationToken)
    {
        HostModel? host = repository.GetHostByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<HostDto>, Error[]>>(
            new ApiNullableResult<HostDto> { Value = host?.ToDto() });
    }
}

internal sealed class CreateHostCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateHostCommand, HostDto>
{
    public Task<OneOf<HostDto, Error[]>> Handle(CreateHostCommand request, CancellationToken cancellationToken)
    {
        HostModel created = repository.CreateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<HostDto, Error[]>>(created.ToDto());
    }
}

internal sealed class UpdateHostCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateHostCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateHostCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateHost(request.Host.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class DeleteHostCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteHostCommand, bool>
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
