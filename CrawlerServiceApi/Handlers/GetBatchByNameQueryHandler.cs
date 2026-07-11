using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.DbModels;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetBatchByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetBatchByNameQuery, ApiNullableResult<BatchDto>>
{
    public Task<OneOf<ApiNullableResult<BatchDto>, Error[]>> Handle(GetBatchByNameQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<BatchDto>, Error[]>>(
            new ApiNullableResult<BatchDto> { Value = batch?.ToDto() });
    }
}