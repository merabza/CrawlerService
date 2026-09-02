using System.Threading;
using System.Threading.Tasks;
using CrawlerService.Application.Repositories;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.TaskModels;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class DeleteTaskCommandHandler(ICrawlerRepository repository) : ICommandHandler<DeleteTaskCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        if (task is null)
        {
            return Task.FromResult<Result<bool>>(CrawlerServiceErrors.TaskWithNameNotFound(request.Name));
        }

        repository.DeleteTask(task);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
