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
public static class TaskEndpoints
{
    public static bool MapTaskEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(MapTaskEndpoints));

        RouteGroupBuilder group = endpoints
            .MapGroup(CrawlerServiceApiRoutes.ApiBase + CrawlerServiceApiRoutes.TaskRoute.TaskBase)
            .RequireAuthorization();

        group.MapGet(CrawlerServiceApiRoutes.TaskRoute.List, GetTasksList);
        group.MapGet(CrawlerServiceApiRoutes.TaskRoute.GetByName, GetTaskByName);
        group.MapPost(CrawlerServiceApiRoutes.TaskRoute.Create, CreateTask);
        group.MapPut(CrawlerServiceApiRoutes.TaskRoute.Update, UpdateTask);
        group.MapDelete(CrawlerServiceApiRoutes.TaskRoute.Delete, DeleteTask);
        group.MapDelete(CrawlerServiceApiRoutes.TaskRoute.ClearFetchedData, ClearTaskFetchedData);

        group.MapGet(CrawlerServiceApiRoutes.TaskRoute.StartPointGet, GetStartPoint);
        group.MapPost(CrawlerServiceApiRoutes.TaskRoute.StartPointAdd, AddStartPoint);
        group.MapPut(CrawlerServiceApiRoutes.TaskRoute.StartPointUpdate, UpdateStartPoint);
        group.MapDelete(CrawlerServiceApiRoutes.TaskRoute.StartPointDelete, DeleteStartPoint);

        debugLogger?.Information("{MethodName} Finished", nameof(MapTaskEndpoints));

        return true;
    }

    private static async Task<IResult> GetTasksList(IMediator mediator, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetTasksListQuery(), cancellationToken)).Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> GetTaskByName(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetTaskByNameQuery(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> CreateTask(IMediator mediator, [FromBody] TaskDto task,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new CreateTaskCommand(task), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> UpdateTask(IMediator mediator, [FromBody] TaskDto task,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new UpdateTaskCommand(task), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> DeleteTask(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new DeleteTaskCommand(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> ClearTaskFetchedData(IMediator mediator, [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new ClearTaskFetchedDataCommand(name), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> GetStartPoint(IMediator mediator, [FromQuery] int taskId,
        [FromQuery] string startPoint, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new GetStartPointQuery(taskId, startPoint), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> AddStartPoint(IMediator mediator, [FromBody] AddStartPointRequest request,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new AddStartPointCommand(request.TaskId, request.StartPoint), cancellationToken))
            .Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> UpdateStartPoint(IMediator mediator, [FromBody] TaskStartPointDto startPoint,
        CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new UpdateStartPointCommand(startPoint), cancellationToken)).Match(Results.Ok,
            Results.BadRequest);
    }

    private static async Task<IResult> DeleteStartPoint(IMediator mediator, [FromQuery] int taskId,
        [FromQuery] string startPoint, CancellationToken cancellationToken = default)
    {
        return (await mediator.Send(new DeleteStartPointCommand(taskId, startPoint), cancellationToken)).Match(
            Results.Ok, Results.BadRequest);
    }
}
