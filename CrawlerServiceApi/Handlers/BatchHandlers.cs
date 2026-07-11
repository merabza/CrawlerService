using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.DbModels;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetBatchesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetBatchesListQuery, List<BatchDto>>
{
    public Task<OneOf<List<BatchDto>, Error[]>> Handle(GetBatchesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<BatchDto>, Error[]>>(repository.GetBatchesList()
            .Select(batch => batch.ToDto()).ToList());
    }
}

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

internal sealed class UpdateBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateBatchCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateBatch(request.Batch.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class DeleteBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteBatchCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.Name);
        if (batch is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.BatchWithNameNotFound(request.Name)
            });
        }

        repository.DeleteBatch(batch);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class GetHostStartUrlNamesByBatchQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetHostStartUrlNamesByBatchQuery, List<string>>
{
    public Task<OneOf<List<string>, Error[]>> Handle(GetHostStartUrlNamesByBatchQuery request,
        CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        return Task.FromResult<OneOf<List<string>, Error[]>>(batch is null
            ? new List<string>()
            : repository.GetHostStartUrlNamesByBatch(batch));
    }
}

internal sealed class AddHostByBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<AddHostByBatchCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(AddHostByBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        if (batch is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.BatchWithNameNotFound(request.BatchName)
            });
        }

        repository.AddHostNamesByBatch(batch, request.SchemeName, request.HostName);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class RemoveHostByBatchCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<RemoveHostByBatchCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(RemoveHostByBatchCommand request, CancellationToken cancellationToken)
    {
        Batch? batch = repository.GetBatchByName(request.BatchName);
        if (batch is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.BatchWithNameNotFound(request.BatchName)
            });
        }

        repository.RemoveHostNamesByBatch(batch, request.SchemeName, request.HostName);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}
