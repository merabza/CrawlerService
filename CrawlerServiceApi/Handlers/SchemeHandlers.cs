using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetSchemesListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetSchemesListQuery, List<SchemeDto>>
{
    public Task<OneOf<List<SchemeDto>, Error[]>> Handle(GetSchemesListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<SchemeDto>, Error[]>>(
            repository.GetSchemesList().Select(scheme => scheme.ToDto()).ToList());
    }
}

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

internal sealed class CreateSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateSchemeCommand, SchemeDto>
{
    public Task<OneOf<SchemeDto, Error[]>> Handle(CreateSchemeCommand request, CancellationToken cancellationToken)
    {
        SchemeModel created = repository.CreateScheme(request.Scheme.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<SchemeDto, Error[]>>(created.ToDto());
    }
}

internal sealed class UpdateSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateSchemeCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateSchemeCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateScheme(request.Scheme.ToEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class DeleteSchemeCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteSchemeCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteSchemeCommand request, CancellationToken cancellationToken)
    {
        SchemeModel? scheme = repository.GetSchemeByName(request.Name);
        if (scheme is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.SchemeWithNameNotFound(request.Name)
            });
        }

        repository.DeleteScheme(scheme);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}
