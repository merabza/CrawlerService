using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceRoot.Domain.TaskStartPoints;
using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerServiceApi.Handlers;

internal sealed class AddStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<AddStartPointCommand, TaskStartPointDto>
{
    public Task<Result<TaskStartPointDto>> Handle(AddStartPointCommand request, CancellationToken cancellationToken)
    {
        TaskStartPoint added = repository.AddStartPoint(request.TaskId, request.StartPoint);
        repository.SaveChanges();
        return Task.FromResult<Result<TaskStartPointDto>>(added.ToDto());
    }
}
