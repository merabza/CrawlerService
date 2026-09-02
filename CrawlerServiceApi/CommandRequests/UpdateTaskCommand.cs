using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record UpdateTaskCommand(TaskDto Task) : ICommand<bool>;
