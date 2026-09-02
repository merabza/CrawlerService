using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class GetSchemesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetSchemesListQuery, List<SchemeDto>>
{
    public Task<Result<List<SchemeDto>>> Handle(GetSchemesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<List<SchemeDto>>>(repository.GetSchemesList().Select(scheme => scheme.ToDto())
            .ToList());
    }
}
