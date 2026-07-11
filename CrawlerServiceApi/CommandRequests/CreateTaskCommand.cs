using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record CreateTaskCommand(TaskDto Task) : ICommand<TaskDto>;