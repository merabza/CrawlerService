using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

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
