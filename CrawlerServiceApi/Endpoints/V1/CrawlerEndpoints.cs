using System;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.DbModels;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.V1.Routes;
using DoCrawler;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
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
        // GET api/v1/crawler/precheck
        group.MapGet(CrawlerServiceApiRoutes.CrawlerRoute.PreCheck, PreCheck);

        debugLogger?.Information("{MethodName} Finished", nameof(MapCrawlerEndpoints));

        return true;
    }

    // POST api/v1/crawler/runbatch
    private static async Task<IResult> RunBatch(HttpRequest httpRequest, IMediator mediator,
        IProgressDataManager progressDataManager, [FromQuery] string? batchName,
        [FromQuery] int newPartsCreateLimit = 0, [FromQuery] int progressDelaySeconds = 1,
        CancellationToken cancellationToken = default)
    {
        string? userName = httpRequest.HttpContext.User.Identity?.Name;
        //კლიენტიდან მოსული დაყოვნება დაიმახსოვროს, რომ პროგრესის ინფორმაცია ამ პერიოდზე ხშირად არ გაიგზავნოს
        progressDataManager.SetSendDelay(TimeSpan.FromSeconds(progressDelaySeconds));
        await progressDataManager.SetProgressData(userName, ReCounterConstants.ProcName, $"{nameof(RunBatch)} started",
            true, cancellationToken);

        var command = new RunBatchCommand(batchName, userName, newPartsCreateLimit);
        OneOf<bool, Error[]> result = await mediator.Send(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/v1/crawler/runtask
    private static async Task<IResult> RunTask(HttpRequest httpRequest, IMediator mediator,
        IProgressDataManager progressDataManager, [FromBody] RunTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        string? userName = httpRequest.HttpContext.User.Identity?.Name;
        //კლიენტიდან მოსული დაყოვნება დაიმახსოვროს, რომ პროგრესის ინფორმაცია ამ პერიოდზე ხშირად არ გაიგზავნოს
        progressDataManager.SetSendDelay(TimeSpan.FromSeconds(request.ProgressDelaySeconds));
        await progressDataManager.SetProgressData(userName, ReCounterConstants.ProcName, $"{nameof(RunTask)} started",
            true, cancellationToken);

        var command = new RunTaskCommand(request.TaskName, userName, request.NewPartsCreateLimit);
        OneOf<bool, Error[]> result = await mediator.Send(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/v1/crawler/testonepage
    private static async Task<IResult> TestOnePage(HttpRequest httpRequest, IMediator mediator,
        IProgressDataManager progressDataManager, [FromBody] TestOnePageRequest request,
        CancellationToken cancellationToken = default)
    {
        string? userName = httpRequest.HttpContext.User.Identity?.Name;
        //კლიენტიდან მოსული დაყოვნება დაიმახსოვროს, რომ პროგრესის ინფორმაცია ამ პერიოდზე ხშირად არ გაიგზავნოს
        progressDataManager.SetSendDelay(TimeSpan.FromSeconds(request.ProgressDelaySeconds));
        await progressDataManager.SetProgressData(userName, ReCounterConstants.ProcName,
            $"{nameof(TestOnePage)} started", true, cancellationToken);

        var command = new TestOnePageCommand(request.TaskName, request.Url, userName, request.DeleteContentForReanalyze,
            request.NewPartsCreateLimit);
        OneOf<bool, Error[]> result = await mediator.Send(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    // GET api/v1/crawler/precheck
    private static Task<IResult> PreCheck(IServiceScopeFactory scopeFactory, [FromQuery] string batchName,
        [FromQuery] string? url)
    {
        // ReSharper disable once using
        using IServiceScope scope = scopeFactory.CreateScope();
        var crawlerRepository = scope.ServiceProvider.GetRequiredService<ICrawlerRepository>();

        var result = new CrawlerPreCheckResult();

        Batch? batch = crawlerRepository.GetBatchByName(batchName);
        if (batch is null)
        {
            return Task.FromResult(Results.Ok(result));
        }

        result.AutoCreateNextPart = batch.AutoCreateNextPart;

        BatchPart? openPart = crawlerRepository.GetOpenedBatchPart(batch.BatchId);
        result.HasOpenPart = openPart is not null;

        if (openPart is not null && !string.IsNullOrWhiteSpace(url))
        {
            result.PageAlreadyAnalyzed = UrlNameHelper.IsPageAlreadyAnalyzed(crawlerRepository, openPart.BpId, url);
        }

        return Task.FromResult(Results.Ok(result));
    }
}
