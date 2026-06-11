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
using OneOf;
using Serilog;
using SystemTools.ReCounterAbstraction;
using SystemTools.ReCounterContracts;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public static class CrawlerEndpoints
{
    public static bool MapCrawlerEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(MapCrawlerEndpoints));

        RouteGroupBuilder group = endpoints
            .MapGroup(CrawlerServiceApiRoutes.ApiBase + CrawlerServiceApiRoutes.CrawlerRoute.CrawlerBase)
            .RequireAuthorization();

        // POST api/v1/crawler/runbatch
        group.MapPost(CrawlerServiceApiRoutes.CrawlerRoute.RunBatch, RunBatch);
        // POST api/v1/crawler/runtask
        group.MapPost(CrawlerServiceApiRoutes.CrawlerRoute.RunTask, RunTask);
        // POST api/v1/crawler/testonepage
        group.MapPost(CrawlerServiceApiRoutes.CrawlerRoute.TestOnePage, TestOnePage);

        debugLogger?.Information("{MethodName} Finished", nameof(MapCrawlerEndpoints));

        return true;
    }

    // POST api/v1/crawler/runbatch
    private static async Task<IResult> RunBatch(HttpRequest httpRequest, IMediator mediator,
        IProgressDataManager progressDataManager, [FromQuery] string? batchName,
        CancellationToken cancellationToken = default)
    {
        string? userName = httpRequest.HttpContext.User.Identity?.Name;
        await progressDataManager.SetProgressData(userName, ReCounterConstants.ProcName, $"{nameof(RunBatch)} started",
            true, cancellationToken);

        var command = new RunBatchCommand(batchName, userName);
        OneOf<bool, Error[]> result = await mediator.Send(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/v1/crawler/runtask
    private static async Task<IResult> RunTask(HttpRequest httpRequest, IMediator mediator,
        IProgressDataManager progressDataManager, [FromBody] RunTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        string? userName = httpRequest.HttpContext.User.Identity?.Name;
        await progressDataManager.SetProgressData(userName, ReCounterConstants.ProcName, $"{nameof(RunTask)} started",
            true, cancellationToken);

        var command = new RunTaskCommand(request.TaskName, request.StartPoints, userName);
        OneOf<bool, Error[]> result = await mediator.Send(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/v1/crawler/testonepage
    private static async Task<IResult> TestOnePage(HttpRequest httpRequest, IMediator mediator,
        IProgressDataManager progressDataManager, [FromBody] TestOnePageRequest request,
        CancellationToken cancellationToken = default)
    {
        string? userName = httpRequest.HttpContext.User.Identity?.Name;
        await progressDataManager.SetProgressData(userName, ReCounterConstants.ProcName,
            $"{nameof(TestOnePage)} started", true, cancellationToken);

        var command = new TestOnePageCommand(request.TaskName, request.Url, request.StartPoints, userName);
        OneOf<bool, Error[]> result = await mediator.Send(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }
}
