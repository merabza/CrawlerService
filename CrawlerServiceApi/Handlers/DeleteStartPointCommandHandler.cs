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

internal sealed class DeleteStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandlerOmd<DeleteStartPointCommand, bool>
{
    public Task<OneOf<bool, ErrorOmd[]>> Handle(DeleteStartPointCommand request, CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        if (startPoint is null)
        {
            return Task.FromResult<OneOf<bool, ErrorOmd[]>>(new[]
            {
                CrawlerServiceErrors.StartPointNotFound(request.TaskId, request.StartPoint)
            });
        }

        repository.DeleteStartPoint(startPoint);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, ErrorOmd[]>>(true);
    }
}
