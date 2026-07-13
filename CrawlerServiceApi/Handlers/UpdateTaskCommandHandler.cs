using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class UpdateTaskCommandHandler(ICrawlerRepository repository) : ICommandHandler<UpdateTaskCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateTask(request.Task.ToUpdateEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}