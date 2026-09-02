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

internal sealed class GetHostByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostByNameQuery, ApiNullableResult<HostDto>>
{
    public Task<Result<ApiNullableResult<HostDto>>> Handle(GetHostByNameQuery request,
        CancellationToken cancellationToken)
    {
        HostModel? host = repository.GetHostByName(request.Name);
        return Task.FromResult<Result<ApiNullableResult<HostDto>>>(
            new ApiNullableResult<HostDto> { Value = host?.ToDto() });
    }
}
