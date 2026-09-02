using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceRoot.Domain.HostModels;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

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
