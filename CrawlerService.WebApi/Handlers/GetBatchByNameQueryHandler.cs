using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceRoot.Domain.Batches;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class GetBatchByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetBatchByNameQuery, ApiNullableResult<BatchDto>>
{
    public Task<Result<ApiNullableResult<BatchDto>>> Handle(GetBatchByNameQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.Name);
        return Task.FromResult<Result<ApiNullableResult<BatchDto>>>(
            new ApiNullableResult<BatchDto> { Value = batch?.ToDto() });
    }
}
