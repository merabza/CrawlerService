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

internal sealed class AddStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<AddStartPointCommand, TaskStartPointDto>
{
    public Task<OneOf<TaskStartPointDto, Error[]>> Handle(AddStartPointCommand request,
        CancellationToken cancellationToken)
    {
        TaskStartPoint added = repository.AddStartPoint(request.TaskId, request.StartPoint);
        repository.SaveChanges();
        return Task.FromResult<OneOf<TaskStartPointDto, Error[]>>(added.ToDto());
    }
}
