using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using CrawlerServiceRoot.Domain.TaskModels;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class CreateTaskCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateTaskCommand, TaskDto>
{
    public Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel created = repository.CreateTask(request.Task.ToCreateEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<TaskDto>>(created.ToDto());
    }
}
