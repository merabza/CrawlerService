using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record UpdateTaskCommand(TaskDto Task) : ICommand<bool>;
