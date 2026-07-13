using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class DeleteTaskCommandHandler(ICrawlerRepository repository) : ICommandHandler<DeleteTaskCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        if (task is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.TaskWithNameNotFound(request.Name)
            });
        }

        repository.DeleteTask(task);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}