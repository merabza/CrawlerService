using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using CrawlerServiceApi.CommandRequests;
using CrawlerServiceApi.Mapping;
using CrawlerServiceShared.Contracts;
using CrawlerServiceShared.Contracts.Errors;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;

namespace CrawlerServiceApi.Handlers;

internal sealed class GetTasksListQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetTasksListQuery, List<TaskDto>>
{
    public Task<OneOf<List<TaskDto>, Error[]>> Handle(GetTasksListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<OneOf<List<TaskDto>, Error[]>>(
            repository.GetTasksList().Select(task => task.ToDto()).ToList());
    }
}

internal sealed class GetTaskByNameQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetTaskByNameQuery, ApiNullableResult<TaskDto>>
{
    public Task<OneOf<ApiNullableResult<TaskDto>, Error[]>> Handle(GetTaskByNameQuery request,
        CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        return Task.FromResult<OneOf<ApiNullableResult<TaskDto>, Error[]>>(
            new ApiNullableResult<TaskDto> { Value = task?.ToDto() });
    }
}

internal sealed class CreateTaskCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<CreateTaskCommand, TaskDto>
{
    public Task<OneOf<TaskDto, Error[]>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel created = repository.CreateTask(request.Task.ToCreateEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<TaskDto, Error[]>>(created.ToDto());
    }
}

internal sealed class UpdateTaskCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateTaskCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateTask(request.Task.ToUpdateEntity());
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class DeleteTaskCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<DeleteTaskCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        TaskModel? task = repository.GetTaskByName(request.Name);
        if (task is null)
        {
            return Task.FromResult<OneOf<bool, Error[]>>(new[] { CrawlerServiceErrors.TaskWithNameNotFound(request.Name) });
        }

        repository.DeleteTask(task);
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

internal sealed class GetStartPointQueryHandler(ICrawlerRepository repository)
    : IQueryHandler<GetStartPointQuery, ApiNullableResult<TaskStartPointDto>>
{
    public Task<OneOf<ApiNullableResult<TaskStartPointDto>, Error[]>> Handle(GetStartPointQuery request,
        CancellationToken cancellationToken)
    {
        TaskStartPoint? startPoint = repository.GetStartPoint(request.TaskId, request.StartPoint);
        return Task.FromResult<OneOf<ApiNullableResult<TaskStartPointDto>, Error[]>>(
            new ApiNullableResult<TaskStartPointDto> { Value = startPoint?.ToDto() });
    }
}

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

internal sealed class UpdateStartPointCommandHandler(ICrawlerRepository repository)
    : ICommandHandler<UpdateStartPointCommand, bool>
{
    public Task<OneOf<bool, Error[]>> Handle(UpdateStartPointCommand request, CancellationToken cancellationToken)
    {
        repository.UpdateStartPoint(new TaskStartPoint
        {
            TspId = request.StartPoint.TspId,
            TaskId = request.StartPoint.TaskId,
            StartPoint = request.StartPoint.StartPoint
        });
        repository.SaveChanges();
        return Task.FromResult<OneOf<bool, Error[]>>(true);
    }
}

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
