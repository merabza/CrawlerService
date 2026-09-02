using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.V1.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Serilog;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public static class SchemeEndpoints
{
    public static bool MapSchemeEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(MapSchemeEndpoints));

        RouteGroupBuilder group = endpoints
            .MapGroup(CrawlerServiceApiRoutes.ApiBase + CrawlerServiceApiRoutes.SchemeRoute.SchemeBase)
            .RequireAuthorization();

        group.MapGet(CrawlerServiceApiRoutes.SchemeRoute.List, GetSchemesList);
        group.MapGet(CrawlerServiceApiRoutes.SchemeRoute.GetByName, GetSchemeByName);
        group.MapPost(CrawlerServiceApiRoutes.SchemeRoute.Create, CreateScheme);
        group.MapPut(CrawlerServiceApiRoutes.SchemeRoute.Update, UpdateScheme);
        group.MapDelete(CrawlerServiceApiRoutes.SchemeRoute.Delete, DeleteScheme);

        debugLogger?.Information("{MethodName} Finished", nameof(MapSchemeEndpoints));

        return true;
    }

    private static async Task<IResult> GetSchemesList(IQueryHandler<GetSchemesListQuery, List<SchemeDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetSchemesListQuery(), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> GetSchemeByName([FromQuery] string name,
        IQueryHandler<GetSchemeByNameQuery, ApiNullableResult<SchemeDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetSchemeByNameQuery(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> CreateScheme([FromBody] SchemeDto scheme,
        ICommandHandler<CreateSchemeCommand, SchemeDto> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new CreateSchemeCommand(scheme), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> UpdateScheme([FromBody] SchemeDto scheme,
        ICommandHandler<UpdateSchemeCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new UpdateSchemeCommand(scheme), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> DeleteScheme([FromQuery] string name,
        ICommandHandler<DeleteSchemeCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new DeleteSchemeCommand(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }
}
