using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceShared.Contracts.Errors;
using DoCrawler;
using DoCrawler.Models;
using DoCrawler.ToolActions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SystemTools.ReCounterAbstraction;
using TaskModel = DoCrawler.Models.TaskModel;

namespace CrawlerServiceReCounters;

public sealed class CrawlerReCounter : ReCounter, ICrawlProgressReporter
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

    //ICrawlProgressReporter — DoCrawler-ის ციკლიდან პროგრესის გადმოცემა ReCounter-ის მექანიზმზე.
    //მონიტორინგის/გაგზავნის შეცდომამ crawl-ი არ უნდა ჩააგდოს, ამიტომ ვიჭერთ exception-ებს
    public async Task SetLength(int length, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetProcLength(length, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred reporting progress length");
        }
    }

    public async ValueTask IncreasePosition(CancellationToken cancellationToken = default)
    {
        try
        {
            await IncreaseProcPosition(true, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred reporting progress position");
        }
    }

    public async Task SetMessage(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            await LogProcMessage(message, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred reporting progress message");
        }
    }

    public async Task SetMessage(string messageName, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            await LogProcMessage(messageName, message, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred reporting progress message");
        }
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
            parseOnePageParameters, batchName, batch, _request.NewPartsCreateLimit, this);
        await toolAction.Run(cancellationToken);
    }

    private async Task RunTask(ICrawlerRepository crawlerRepository, ParseOnePageParameters parseOnePageParameters,
        CancellationToken cancellationToken)
    {
        var task = new TaskModel { StartPoints = _request.StartPoints };
        // ReSharper disable once using
        var toolAction = new CrawlerRunnerToolAction(_logger, _httpClientFactory, crawlerRepository, _crawlerParameters,
            parseOnePageParameters, _request.TaskName ?? string.Empty, task, null, _request.NewPartsCreateLimit, this);
        await toolAction.Run(cancellationToken);
    }

    private async Task RunOnePage(ICrawlerRepository crawlerRepository, ParseOnePageParameters parseOnePageParameters,
        CancellationToken cancellationToken)
    {
        var task = new TaskModel { StartPoints = _request.StartPoints };
        // ReSharper disable once using
        var toolAction = new OnePageCrawlerRunnerToolAction(_logger, _httpClientFactory, crawlerRepository,
            _crawlerParameters, parseOnePageParameters, _request.TaskName ?? "TestOnePage", task,
            _request.Url ?? string.Empty, _request.NewPartsCreateLimit, _request.DeleteContentForReanalyze);
        await toolAction.Run(cancellationToken);
    }
}
