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
public static class HostEndpoints
{
    public static bool MapHostEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(MapHostEndpoints));

        RouteGroupBuilder group = endpoints
            .MapGroup(CrawlerServiceApiRoutes.ApiBase + CrawlerServiceApiRoutes.HostRoute.HostBase)
            .RequireAuthorization();

        group.MapGet(CrawlerServiceApiRoutes.HostRoute.List, GetHostsList);
        group.MapGet(CrawlerServiceApiRoutes.HostRoute.GetByName, GetHostByName);
        group.MapPost(CrawlerServiceApiRoutes.HostRoute.Create, CreateHost);
        group.MapPut(CrawlerServiceApiRoutes.HostRoute.Update, UpdateHost);
        group.MapDelete(CrawlerServiceApiRoutes.HostRoute.Delete, DeleteHost);

        debugLogger?.Information("{MethodName} Finished", nameof(MapHostEndpoints));

        return true;
    }

    private static async Task<IResult> GetHostsList(IMediator mediator, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetHostsListQuery(), cancellationToken)).Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> GetHostByName(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetHostByNameQuery(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> CreateHost(IMediator mediator, [FromBody] HostDto host,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new CreateHostCommand(host), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> UpdateHost(IMediator mediator, [FromBody] HostDto host,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new UpdateHostCommand(host), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> DeleteHost(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new DeleteHostCommand(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }
}
