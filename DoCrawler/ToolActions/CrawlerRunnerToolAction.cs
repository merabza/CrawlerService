using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using DoCrawler.Models;
using Microsoft.Extensions.Logging;
using TaskModel = DoCrawler.Models.TaskModel;

namespace DoCrawler.ToolActions;

public sealed class CrawlerRunnerToolAction : CrawlerToolAction
{
    private readonly Batch? _batch;

    //private readonly ICrawlerRepositoryCreatorFactory _crawlerRepositoryCreatorFactory;

    public CrawlerRunnerToolAction(ILogger logger, IHttpClientFactory httpClientFactory,
        ICrawlerRepository crawlerRepository, CrawlerParameters par, ParseOnePageParameters parseOnePageParameters,
        string taskName, TaskModel? task, Batch? batch, int newPartsCreateLimit) : base(logger, par, taskName, task,
        crawlerRepository, httpClientFactory, parseOnePageParameters, newPartsCreateLimit)
    {
        _batch = batch;
    }

    public CrawlerRunnerToolAction(ILogger logger, IHttpClientFactory httpClientFactory,
        ICrawlerRepository crawlerRepository, CrawlerParameters par, ParseOnePageParameters parseOnePageParameters,
        string taskName, Batch? batch, int newPartsCreateLimit) : base(logger, par, taskName, null, crawlerRepository,
        httpClientFactory, parseOnePageParameters, newPartsCreateLimit)
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
