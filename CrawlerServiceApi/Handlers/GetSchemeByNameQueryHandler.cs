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

internal sealed class GetSchemeByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetSchemeByNameQuery, ApiNullableResult<SchemeDto>>
{
    public Task<OneOf<ApiNullableResult<SchemeDto>, Error[]>> Handle(GetSchemeByNameQuery request,
        CancellationToken cancellationToken)
    {
        SchemeModel? scheme = repository.GetSchemeByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<SchemeDto>, Error[]>>(
            new ApiNullableResult<SchemeDto> { Value = scheme?.ToDto() });
    }
}