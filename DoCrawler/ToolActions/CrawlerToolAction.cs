using System;
using System.Linq;
using System.Net.Http;
using CrawlerDbModels;
using CrawlerDbPersistence.Configurations;
using CrawlerRepoInterfaces;
using DoCrawler.Models;
using Microsoft.Extensions.Logging;
using RobotsTxt;
using SystemTools.BackgroundTasks;
using SystemTools.SystemToolsShared;
using TaskModel = DoCrawler.Models.TaskModel;

namespace DoCrawler.ToolActions;

public /*open*/ class CrawlerToolAction : ToolAction
{
    private readonly ICrawlerRepository _crawlerRepository;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ParseOnePageParameters _parseOnePageParameters;
    private readonly ICrawlProgressReporter? _progressReporter;
    private readonly string? _taskName;
    protected readonly ILogger CrLogger;
    protected readonly CrawlerParameters Par;
    protected readonly TaskModel? Task;

    //ციკლში ახალი ნაწილების შესაქმნელად დარჩენილი ლიმიტი: 0 = აღარ შეიქმნას, -1 = შეუზღუდავად
    private int _newPartsCreateLimit;

    protected CrawlerToolAction(ILogger logger, CrawlerParameters par, string taskName, TaskModel? task,
        ICrawlerRepository crawlerRepository, IHttpClientFactory httpClientFactory,
        ParseOnePageParameters parseOnePageParameters, int newPartsCreateLimit,
        ICrawlProgressReporter? progressReporter = null) : base(logger, taskName, null, null, true)
    {
        CrLogger = logger;
        Par = par;
        _taskName = taskName;
        Task = task;
        _httpClientFactory = httpClientFactory;
        _parseOnePageParameters = parseOnePageParameters;
        _crawlerRepository = crawlerRepository;
        _newPartsCreateLimit = newPartsCreateLimit;
        _progressReporter = progressReporter;
    }

    protected BatchPartRunner? CreateBatchPartRunner(BatchPart? batchPart, Batch batch)
    {
        BatchPartRunner? batchPartRunner = null;

        bool createNewPart = false;
        if (batchPart == null)
        {
            createNewPart = IsCreateNewPartAllowed(batch);
            if (!createNewPart)
            {
                return null;
            }
        }

        if (createNewPart)
        {
            batchPart = _crawlerRepository.TryCreateNewPart(batch.BatchId);
            _crawlerRepository.SaveChanges();
        }

        if (batchPart is not null)
        {
            batchPartRunner = new BatchPartRunner(CrLogger, _httpClientFactory, _crawlerRepository, Par,
                _parseOnePageParameters, batchPart, _progressReporter);
        }

        if (batchPartRunner is null)
        {
            CrLogger.LogError("batchPartRunner is null");
            return null;
        }

        //if (createNewPart)
        //    batchPartRunner.InitBachPart(Task?.StartPoints ?? [], batch);
        return batchPartRunner;
    }

    private Batch? GetBatchByTaskName(ICrawlerRepository crawlerRepository)
    {
        if (_taskName == null || Task == null || Task.StartPoints.Count == 0)
        {
            CrLogger.LogError("Not enough Information About Task");
            return null;
        }

        string? newBatchName = _taskName.Truncate(BatchConfiguration.BatchNameLength);
        if (newBatchName is null)
        {
            CrLogger.LogError("Invalid task name for new batch");
            return null;
        }

        //მოიძებნოს ბაზაში Batch სახელით _taskName
        //თუ არ არსებობს Batch სახელით _taskName, შეიქმნას IsOpen=false, AutoCreateNextPart=false
        Batch? batch = crawlerRepository.GetBatchByName(_taskName);
        if (batch == null)
        {
            batch = crawlerRepository.CreateBatch(new Batch
            {
                BatchName = newBatchName, IsOpen = false, AutoCreateNextPart = false
            });
            crawlerRepository.SaveChanges();
        }

        //მოხდეს _task.StartPoints-ების განხილვა. თითოეულისათვის:
        //გამოიყოს საწყისი მისამართიდან ჰოსტი და სქემა,
        //შემოწმდეს და თუ არ არსებობს ბაზაში ასეთი ჰოსტი ან სქემა, დარეგისტრირდეს თითოეული.
        //ამ სქემისა და ჰოსტის წყვილისთვის შემოწმდეს არის თუ არა დარეგისტრირებული HostByBatch ცხრილში
        //თუ არ არის დარეგისტრირებული, დარეგისტრირდეს.
        foreach (Uri? myUri in Task.StartPoints.Select(UriFactory.GetUri).Where(myUri => myUri != null))
        {
            crawlerRepository.AddHostNamesByBatch(batch, myUri!.Scheme, myUri.Host);
        }

        crawlerRepository.SaveChanges();

        string batchName = batch.BatchName;
        if (CrLogger.IsEnabled(LogLevel.Information))
        {
            CrLogger.LogInformation("Crawling for batch {BatchName}", batchName);
        }

        return batch;
    }

    private bool IsCreateNewPartAllowed(Batch batch)
    {
        //გადაწყვეტილება მიღებულია CrawlerConsole-ის მხარეს და გადმოცემულია NewPartsCreateLimit-ით
        if (batch.AutoCreateNextPart)
        {
            return true;
        }

        if (_newPartsCreateLimit < 0)
        {
            return true;
        }

        if (_newPartsCreateLimit > 0)
        {
            _newPartsCreateLimit--;
            return true;
        }

        return false;
    }

    protected (Batch?, BatchPart?) PrepareBatchPart(Batch? startBatch = null)
    {
        var par = ParseOnePageParameters.Create(Par);
        if (par is null)
        {
            StShared.WriteErrorLine("ParseOnePageParameters does not created", true, CrLogger, false);
            return (null, null);
        }

        Batch? batch = startBatch ?? GetBatchByTaskName(_crawlerRepository);

        if (batch is not null)
        {
            return (batch, _crawlerRepository.GetOpenedBatchPart(batch.BatchId));
        }

        StShared.WriteErrorLine("batch is null", true, CrLogger, false);
        return (null, null);
    }
}
