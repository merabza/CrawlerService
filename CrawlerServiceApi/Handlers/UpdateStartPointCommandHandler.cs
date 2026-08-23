using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<UpdateStartPointCommand, bool>
{
    public Task<OneOf<bool, ErrorOmd[]>> Handle(UpdateStartPointCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateStartPoint(new TaskStartPoint
        {
            TspId = request.StartPoint.TspId,
            TaskId = request.StartPoint.TaskId,
            StartPoint = request.StartPoint.StartPoint
        });
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, ErrorOmd[]>>(true);
    }
}
