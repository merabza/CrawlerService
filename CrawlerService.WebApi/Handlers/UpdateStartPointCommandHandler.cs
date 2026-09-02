using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.TaskStartPoints;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class UpdateStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateStartPointCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateStartPointCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateStartPoint(new TaskStartPoint
        {
            TspId = request.StartPoint.TspId,
            TaskId = request.StartPoint.TaskId,
            StartPoint = request.StartPoint.StartPoint
        });
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
