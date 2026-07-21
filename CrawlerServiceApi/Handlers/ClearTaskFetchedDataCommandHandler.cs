using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts.Errors;
using Microsoft.EntityFrameworkCore.Storage;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class ClearTaskFetchedDataCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<ClearTaskFetchedDataCommand, bool>
{
    public async Task<OneOf<bool, Error[]>> Handle(ClearTaskFetchedDataCommand request,
        CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        if (task is null)
        {
            return new[] { CrawlerServiceErrors.TaskWithNameNotFound(request.Name) };
        }

        //Batch იქმნება ამოცანის სახელით პირველი გაშვებისას; თუ არ არსებობს, გასასუფთავებელი არაფერია
        Batch? batch = repository.GetBatchByName(request.Name);
        if (batch is null)
        {
            return true;
        }

        //თუ Commit-მდე რომელიმე ნაბიჯი ჩავარდა, using-ის dispose ტრანზაქციას უკან აბრუნებს
        await using IDbContextTransaction transaction = repository.GetTransaction();

        //ექსკლუზიური ჰოსტების სია უნდა დადგინდეს Batch-ის წაშლამდე, რადგან წაშლა HostsByBatches-საც შლის
        List<int> exclusiveHostIds = repository.GetBatchExclusiveHostIds(batch.BatchId);

        //ჯერ Batch (cascade შლის BatchParts-ს და მათზე მიბმულ გრაფს, ანალიზს, ტერმინებს, რობოტებს),
        //მხოლოდ ამის შემდეგ Urls-ები, რადგან ამ Batch-ის გრაფის კვანძები Urls-ებზე მიუთითებენ
        repository.DeleteBatch(batch);
        repository.SaveChanges();
        repository.DeleteUrlsByHostIds(exclusiveHostIds);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}