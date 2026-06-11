using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDb.Models;
using CrawlerServiceShared.Contracts.Errors;
using DoCrawler.Domain;
using DoCrawler.Models;
using DoCrawler.ToolActions;
using LibCrawlerRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SystemTools.ReCounterAbstraction;

namespace CrawlerServiceReCounters;

public sealed class CrawlerReCounter : ReCounter
{
    private const string SuperName = "Crawler";
    private const string ProcessName = "Crawling";

    private readonly CrawlerParameters _crawlerParameters;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly CrawlRequest _request;
    private readonly IServiceScopeFactory _scopeFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public CrawlerReCounter(IServiceScopeFactory scopeFactory, IConfiguration configuration,
        IProgressDataManager progressDataManager, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory,
        CrawlerParameters crawlerParameters, CrawlRequest request) : base(request.UserName, SuperName, ProcessName,
        progressDataManager, configuration["DatabaseReCounterSettings:ReCounterLogsFolderName"])
    {
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
        _logger = loggerFactory.CreateLogger<CrawlerReCounter>();
        _crawlerParameters = crawlerParameters;
        _request = request;
    }

    protected override async Task RunRecount(CancellationToken cancellationToken = default)
    {
        var parseOnePageParameters = ParseOnePageParameters.Create(_crawlerParameters);
        if (parseOnePageParameters is null)
        {
            await LogErrors([CrawlerServiceErrors.ParseOnePageParametersNotCreated], cancellationToken);
            return;
        }

        // ReSharper disable once using
        using IServiceScope scope = _scopeFactory.CreateScope();
        var crawlerRepository = scope.ServiceProvider.GetRequiredService<ICrawlerRepository>();

        switch (_request.Kind)
        {
            case ECrawlKind.Batch:
                await RunBatch(crawlerRepository, parseOnePageParameters, cancellationToken);
                break;
            case ECrawlKind.Task:
                await RunTask(crawlerRepository, parseOnePageParameters, cancellationToken);
                break;
            case ECrawlKind.OnePage:
                await RunOnePage(crawlerRepository, parseOnePageParameters, cancellationToken);
                break;
            default:
                await LogProcMessage($"Unsupported crawl kind {_request.Kind}", cancellationToken);
                break;
        }
    }

    private async Task RunBatch(ICrawlerRepository crawlerRepository, ParseOnePageParameters parseOnePageParameters,
        CancellationToken cancellationToken)
    {
        string batchName = _request.BatchName ?? string.Empty;
        Batch? batch = crawlerRepository.GetBatchByName(batchName);
        if (batch is null)
        {
            await LogErrors([CrawlerServiceErrors.BatchWithNameNotFound(batchName)], cancellationToken);
            return;
        }

        // ReSharper disable once using
        var toolAction = new CrawlerRunnerToolAction(_logger, _httpClientFactory, crawlerRepository, _crawlerParameters,
            parseOnePageParameters, batchName, batch);
        await toolAction.Run(cancellationToken);
    }

    private async Task RunTask(ICrawlerRepository crawlerRepository, ParseOnePageParameters parseOnePageParameters,
        CancellationToken cancellationToken)
    {
        var task = new TaskModel { StartPoints = _request.StartPoints };
        // ReSharper disable once using
        var toolAction = new CrawlerRunnerToolAction(_logger, _httpClientFactory, crawlerRepository, _crawlerParameters,
            parseOnePageParameters, _request.TaskName ?? string.Empty, task, null);
        await toolAction.Run(cancellationToken);
    }

    private async Task RunOnePage(ICrawlerRepository crawlerRepository, ParseOnePageParameters parseOnePageParameters,
        CancellationToken cancellationToken)
    {
        var task = new TaskModel { StartPoints = _request.StartPoints };
        // ReSharper disable once using
        var toolAction = new OnePageCrawlerRunnerToolAction(_logger, _httpClientFactory, crawlerRepository,
            _crawlerParameters, parseOnePageParameters, _request.TaskName ?? "TestOnePage", task,
            _request.Url ?? string.Empty);
        await toolAction.Run(cancellationToken);
    }
}
