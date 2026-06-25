using System.Collections.Generic;
using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record GetTasksListQuery : IQuery<List<TaskDto>>;

public sealed record GetTaskByNameQuery(string Name) : IQuery<ApiNullableResult<TaskDto>>;

public sealed record CreateTaskCommand(TaskDto Task) : ICommand<TaskDto>;

public sealed record UpdateTaskCommand(TaskDto Task) : ICommand<bool>;

public sealed record DeleteTaskCommand(string Name) : ICommand<bool>;

public sealed record GetStartPointQuery(int TaskId, string StartPoint) : IQuery<ApiNullableResult<TaskStartPointDto>>;

public sealed record AddStartPointCommand(int TaskId, string StartPoint) : ICommand<TaskStartPointDto>;

public sealed record UpdateStartPointCommand(TaskStartPointDto StartPoint) : ICommand<bool>;

public sealed record DeleteStartPointCommand(int TaskId, string StartPoint) : ICommand<bool>;
