using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class CreateTaskCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<CreateTaskCommand, TaskDto>
{
    public Task<OneOf<TaskDto, ErrorOmd[]>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel created = repository.CreateTask(request.Task.ToCreateEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<TaskDto, ErrorOmd[]>>(created.ToDto());
    }
}
