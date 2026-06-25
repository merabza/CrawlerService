using System.Threading;
using System.Threading.Tasks;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.V1.Routes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace CrawlerServiceApi.Endpoints.V1;

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

    private static async Task<IResult> GetSchemesList(IMediator mediator, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetSchemesListQuery(), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> GetSchemeByName(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetSchemeByNameQuery(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> CreateScheme(IMediator mediator, [FromBody] SchemeDto scheme,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new CreateSchemeCommand(scheme), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> UpdateScheme(IMediator mediator, [FromBody] SchemeDto scheme,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new UpdateSchemeCommand(scheme), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> DeleteScheme(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new DeleteSchemeCommand(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }
}
