using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetSchemesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetSchemesListQuery, List<SchemeDto>>
{
    public Task<OneOf<List<SchemeDto>, Error[]>> Handle(GetSchemesListQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<SchemeDto>, Error[]>>(repository.GetSchemesList()
            .Select(scheme => scheme.ToDto()).ToList());
    }
}
