using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using DoCrawler.Models;
using Microsoft.Extensions.Logging;

namespace DoCrawler.ToolActions;

public sealed class CrawlerRunnerToolAction : CrawlerToolAction
{
    private readonly Batch? _batch;

    //private readonly ICrawlerRepositoryCreatorFactory _crawlerRepositoryCreatorFactory;

    public CrawlerRunnerToolAction(ILogger logger, IHttpClientFactory httpClientFactory,
        ICrawlerRepository crawlerRepository, CrawlerParameters par, ParseOnePageParameters parseOnePageParameters,
        string taskName, DoCrawlerTaskModel? task, Batch? batch, int newPartsCreateLimit,
        ICrawlProgressReporter? progressReporter = null) : base(logger, par, taskName, task, crawlerRepository,
        httpClientFactory, parseOnePageParameters, newPartsCreateLimit, progressReporter)
    {
        _batch = batch;
    }

    public CrawlerRunnerToolAction(ILogger logger, IHttpClientFactory httpClientFactory,
        ICrawlerRepository crawlerRepository, CrawlerParameters par, ParseOnePageParameters parseOnePageParameters,
        string taskName, Batch? batch, int newPartsCreateLimit, ICrawlProgressReporter? progressReporter = null) : base(
        logger, par, taskName, null, crawlerRepository, httpClientFactory, parseOnePageParameters, newPartsCreateLimit,
        progressReporter)
    {
        _batch = batch;
    }

    protected override async ValueTask<bool> RunAction(CancellationToken cancellationToken = default)
    {
        //1. start
        (Batch? batch, BatchPart? batchPart) = PrepareBatchPart(_batch);

        if (batch is null)
        {
            return false;
        }
        //1. Finish

        while (true)
        {
            //თუ მოთხოვნილია პროცესის შეჩერება, გამოვიდეთ მეთოდიდან
            if (cancellationToken.IsCancellationRequested)
            {
                return true;
            }

            //2. Start
            BatchPartRunner? batchPartRunner = CreateBatchPartRunner(batchPart, batch);
            //2. Finish
            if (batchPartRunner is null)
            {
                return false;
            }

            batchPartRunner.InitBachPart(Task?.StartPoints ?? [], batch);

            await batchPartRunner.RunBatchPart(cancellationToken);

            batchPart = null;
        }
    }
}
