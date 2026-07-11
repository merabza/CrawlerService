using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDomain.RepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceReCounters;
using CrawlerServiceShared.Contracts.Errors;
using DoCrawler.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.ReCounterAbstraction;
using SystemTools.SystemToolsShared.Errors;
using TaskModel = CrawlerDomain.DbModels.TaskModel;

namespace CrawlerServiceApi.Handlers;

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

    public Task<OneOf<bool, Error[]>> Handle(RunTaskCommand request, CancellationToken cancellationToken)
    {
        List<string> startPoints;
        using (IServiceScope scope = _scopeFactory.CreateScope())
        {
            var crawlerRepository = scope.ServiceProvider.GetRequiredService<ICrawlerRepository>();
            TaskModel? task = request.TaskName is null ? null : crawlerRepository.GetTaskByName(request.TaskName);
            if (task is null)
            {
                return Task.FromResult<OneOf<bool, Error[]>>(new[]
                {
                    CrawlerServiceErrors.TaskWithNameNotFound(request.TaskName)
                });
            }

            startPoints = task.StartPoints.Select(sp => sp.StartPoint).ToList();
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
        return Task.FromResult<OneOf<bool, Error[]>>(true);
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
