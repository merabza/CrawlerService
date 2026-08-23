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

internal sealed class DeleteTaskCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<DeleteTaskCommand, bool>
{
    public Task<OneOf<bool, ErrorOmd[]>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        if (task is null)
        {
            return Task.FromResult<OneOf<bool, ErrorOmd[]>>(new[]
            {
                CrawlerServiceErrors.TaskWithNameNotFound(request.Name)
            });
        }

        repository.DeleteTask(task);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, ErrorOmd[]>>(true);
    }
}
