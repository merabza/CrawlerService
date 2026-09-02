using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetSchemesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetSchemesListQuery, List<SchemeDto>>
{
    public Task<Result<List<SchemeDto>>> Handle(GetSchemesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<SchemeDto>>>(repository.GetSchemesList().Select(scheme => scheme.ToDto())
            .ToList());
    }
}
