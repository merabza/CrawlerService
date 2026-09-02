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

    private static async Task<IResult> GetTasksList(IQueryHandler<GetTasksListQuery, List<TaskDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetTasksListQuery(), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> GetTaskByName([FromQuery] string name,
        IQueryHandler<GetTaskByNameQuery, ApiNullableResult<TaskDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetTaskByNameQuery(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> CreateTask([FromBody] TaskDto task,
        ICommandHandler<CreateTaskCommand, TaskDto> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new CreateTaskCommand(task), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> UpdateTask([FromBody] TaskDto task,
        ICommandHandler<UpdateTaskCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new UpdateTaskCommand(task), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> DeleteTask([FromQuery] string name,
        ICommandHandler<DeleteTaskCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new DeleteTaskCommand(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> ClearTaskFetchedData([FromQuery] string name,
        ICommandHandler<ClearTaskFetchedDataCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new ClearTaskFetchedDataCommand(name), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> GetStartPoint([FromQuery] int taskId, [FromQuery] string startPoint,
        IQueryHandler<GetStartPointQuery, ApiNullableResult<TaskStartPointDto>> handler,
        CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new GetStartPointQuery(taskId, startPoint), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> AddStartPoint([FromBody] AddStartPointRequest request,
        ICommandHandler<AddStartPointCommand, TaskStartPointDto> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new AddStartPointCommand(request.TaskId, request.StartPoint), cancellationToken))
            .Match(Results.Ok, failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> UpdateStartPoint([FromBody] TaskStartPointDto startPoint,
        ICommandHandler<UpdateStartPointCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new UpdateStartPointCommand(startPoint), cancellationToken)).Match(Results.Ok,
            failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }

    private static async Task<IResult> DeleteStartPoint([FromQuery] int taskId, [FromQuery] string startPoint,
        ICommandHandler<DeleteStartPointCommand, bool> handler, CancellationToken cancellationToken = default)
    {
        return (await handler.Handle(new DeleteStartPointCommand(taskId, startPoint), cancellationToken)).Match(
            Results.Ok, failure => Results.BadRequest(failure.Error.ToErrorArray()));
    }
}
