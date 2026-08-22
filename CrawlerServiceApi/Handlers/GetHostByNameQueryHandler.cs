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

internal sealed class GetHostByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostByNameQuery, ApiNullableResult<HostDto>>
{
    public Task<OneOf<ApiNullableResult<HostDto>, ErrorOmd[]>> Handle(GetHostByNameQuery request,
        CancellationToken cancellationToken)
    {
        HostModel? host = repository.GetHostByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<HostDto>, ErrorOmd[]>>(
            new ApiNullableResult<HostDto> { Value = host?.ToDto() });
    }
}
