using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.V1.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Serilog;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

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

    private static async Task<IResult> GetBatchesList(IQueryHandler<GetBatchesListQuery, List<BatchDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetBatchesListQuery(), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> GetBatchByName([FromQuery] string name,
        IQueryHandler<GetBatchByNameQuery, ApiNullableResult<BatchDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetBatchByNameQuery(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> CreateBatch([FromBody] BatchDto batch,
        ICommandHandler<CreateBatchCommand, BatchDto> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new CreateBatchCommand(batch), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> UpdateBatch([FromBody] BatchDto batch,
        ICommandHandler<UpdateBatchCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new UpdateBatchCommand(batch), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> DeleteBatch([FromQuery] string name,
        ICommandHandler<DeleteBatchCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new DeleteBatchCommand(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> GetHostStartUrlNamesByBatch([FromQuery] string batchName,
        IQueryHandler<GetHostStartUrlNamesByBatchQuery, List<string>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetHostStartUrlNamesByBatchQuery(batchName), cancellationToken)).Match(
            Results.Ok, failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> AddHostByBatch([FromBody] HostByBatchRequest request,
        ICommandHandler<AddHostByBatchCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new AddHostByBatchCommand(request.BatchName, request.SchemeName, request.HostName),
            cancellationToken)).Match(Results.Ok, failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> RemoveHostByBatch([FromQuery] string batchName, [FromQuery] string schemeName,
        [FromQuery] string hostName, ICommandHandler<RemoveHostByBatchCommand, bool> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new RemoveHostByBatchCommand(batchName, schemeName, hostName), cancellationToken))
            .Match(Results.Ok, failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }
}
