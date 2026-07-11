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

internal sealed class CreateBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateBatchCommand, BatchDto>
{
    public Task<OneOf<BatchDto, Error[]>> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        Batch created = repository.CreateBatch(request.Batch.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<BatchDto, Error[]>>(created.ToDto());
    }
}