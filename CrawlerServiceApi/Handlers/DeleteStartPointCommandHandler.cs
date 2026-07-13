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
    : ICommandHandler<DeleteStartPointCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteStartPointCommand request, CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        if (startPoint is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[]
            {
                CrawlerServiceErrors.StartPointNotFound(request.TaskId, request.StartPoint)
            });
        }

        repository.DeleteStartPoint(startPoint);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}