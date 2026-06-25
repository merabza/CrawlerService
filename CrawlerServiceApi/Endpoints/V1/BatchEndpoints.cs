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
public static class BatchEndpoints
{
    public static bool MapBatchEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(MapBatchEndpoints));

        RouteGroupBuilder group = endpoints
            .MapGroup(CrawlerServiceApiRoutes.ApiBase + CrawlerServiceApiRoutes.BatchRoute.BatchBase)
            .RequireAuthorization();

        group.MapGet(CrawlerServiceApiRoutes.BatchRoute.List, GetBatchesList);
        group.MapGet(CrawlerServiceApiRoutes.BatchRoute.GetByName, GetBatchByName);
        group.MapPost(CrawlerServiceApiRoutes.BatchRoute.Create, CreateBatch);
        group.MapPut(CrawlerServiceApiRoutes.BatchRoute.Update, UpdateBatch);
        group.MapDelete(CrawlerServiceApiRoutes.BatchRoute.Delete, DeleteBatch);

        group.MapGet(CrawlerServiceApiRoutes.BatchRoute.HostByBatchList, GetHostStartUrlNamesByBatch);
        group.MapPost(CrawlerServiceApiRoutes.BatchRoute.HostByBatchAdd, AddHostByBatch);
        group.MapDelete(CrawlerServiceApiRoutes.BatchRoute.HostByBatchRemove, RemoveHostByBatch);

        debugLogger?.Information("{MethodName} Finished", nameof(MapBatchEndpoints));

        return true;
    }

    private static async Task<IResult> GetBatchesList(IMediator mediator, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetBatchesListQuery(), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> GetBatchByName(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetBatchByNameQuery(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> CreateBatch(IMediator mediator, [FromBody] BatchDto batch,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new CreateBatchCommand(batch), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> UpdateBatch(IMediator mediator, [FromBody] BatchDto batch,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new UpdateBatchCommand(batch), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> DeleteBatch(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new DeleteBatchCommand(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> GetHostStartUrlNamesByBatch(IMediator mediator, [FromQuery] string batchName,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetHostStartUrlNamesByBatchQuery(batchName), cancellationToken)).Match(
            Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> AddHostByBatch(IMediator mediator, [FromBody] HostByBatchRequest request,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new AddHostByBatchCommand(request.BatchName, request.SchemeName, request.HostName),
            cancellationToken)).Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> RemoveHostByBatch(IMediator mediator, [FromQuery] string batchName,
        [FromQuery] string schemeName, [FromQuery] string hostName, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new RemoveHostByBatchCommand(batchName, schemeName, hostName), cancellationToken))
            .Match(Results.Ok, Results.BadRequest);
    }
}
