using System.Threading;
using System.Threading.Tasks;
using CrawlerRepoInterfaces;
using CrawlerService.WebApi.CommandRequests;
using CrawlerService.WebApi.Mapping;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;

namespace CrawlerService.WebApi.Handlers;

internal sealed class UpdateTaskCommandHandler(ICrawlerRepository repository) : ICommandHandler<UpdateTaskCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateTask(request.Task.ToUpdateEntity());
        repository.SaveChanges();
        return Task.FromResult<Result<bool>>(true);
    }
}
