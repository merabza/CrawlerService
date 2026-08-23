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

internal sealed class GetBatchByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandlerOmd<GetBatchByNameQuery, ApiNullableResult<BatchDto>>
{
    public Task<OneOf<ApiNullableResult<BatchDto>, ErrorOmd[]>> Handle(GetBatchByNameQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<BatchDto>, ErrorOmd[]>>(
            new ApiNullableResult<BatchDto> { Value = batch?.ToDto() });
    }
}
