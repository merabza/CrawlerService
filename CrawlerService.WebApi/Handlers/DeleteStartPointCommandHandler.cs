using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerServiceRoot.Domain.TaskStartPoints;
using CrawlerServiceShared.Contracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class DeleteStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteStartPointCommand, bool>
{
    public Task<Result<bool>> Handle(DeleteStartPointCommand request, CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        if (startPoint is null)
        {
            return Task.FromResult<Result<bool>>(
                CrawlerServiceErrors.StartPointNotFound(request.TaskId, request.StartPoint));
        }

        repository.DeleteStartPoint(startPoint);
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
