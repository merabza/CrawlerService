using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceRoot.Domain.SchemeModels;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetSchemeByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetSchemeByNameQuery, ApiNullableResult<SchemeDto>>
{
    public Task<Result<ApiNullableResult<SchemeDto>>> Handle(GetSchemeByNameQuery request,
        CancellationToken cancellationToken)
    {
        SchemeModel? scheme = repository.GetSchemeByName(request.Name);
        return Task.FromResult<Result<ApiNullableResult<SchemeDto>>>(
            new ApiNullableResult<SchemeDto> { Value = scheme?.ToDto() });
    }
}
