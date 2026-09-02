using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Crawling.Models;
using CrawlerService.Application.ReCounters;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.TaskModels;
using CrawlerServiceShared.Contracts.Errors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.ReCounterAbstraction;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class RunTaskCommandHandler : ICommandHandler<RunTaskCommand, bool>
{
    private readonly IReCounterBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IConfiguration _configuration;
    private readonly CrawlerParameters _crawlerParameters;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IProgressDataManager _progressDataManager;
    private readonly IServiceScopeFactory _scopeFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RunTaskCommandHandler(IServiceScopeFactory scopeFactory, IReCounterBackgroundTaskQueue backgroundTaskQueue,
        IConfiguration configuration, IProgressDataManager progressDataManager, IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory, CrawlerParameters crawlerParameters)
    {
        _scopeFactory = scopeFactory;
        _backgroundTaskQueue = backgroundTaskQueue;
        _configuration = configuration;
        _progressDataManager = progressDataManager;
        _httpClientFactory = httpClientFactory;
        _loggerFactory = loggerFactory;
        _crawlerParameters = crawlerParameters;
    }

    public Task<Result<bool>> Handle(RunTaskCommand request, CancellationToken cancellationToken)
    {
        List<string> startPoints;
        using (IServiceScope scope = _scopeFactory.CreateScope())
        {
            var crawlerRepository = scope.ServiceProvider.GetRequiredService<ICrawlerRepository>();
            TaskModel? task = request.TaskName is null ? null : crawlerRepository.GetTaskByName(request.TaskName);
            if (task is null)
            {
                return Task.FromResult<Result<bool>>(CrawlerServiceErrors.TaskWithNameNotFound(request.TaskName));
            }

            startPoints = [.. task.StartPoints.Select(sp => sp.StartPoint)];
        }

        var crawlRequest = new CrawlRequest
        {
            Kind = ECrawlKind.Task,
            TaskName = request.TaskName,
            StartPoints = startPoints,
            UserName = request.UserName,
            NewPartsCreateLimit = request.NewPartsCreateLimit
        };
        _backgroundTaskQueue.QueueBackgroundWorkItem(token => Run(crawlRequest, token));
        return Task.FromResult<Result<bool>>(true);
    }

    private Task Run(CrawlRequest crawlRequest, CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            var crawlerReCounter = new CrawlerReCounter(_scopeFactory, _configuration, _progressDataManager,
                _httpClientFactory, _loggerFactory, _crawlerParameters, crawlRequest);
            await crawlerReCounter.Recount(cancellationToken);
        }, cancellationToken);
    }
}
