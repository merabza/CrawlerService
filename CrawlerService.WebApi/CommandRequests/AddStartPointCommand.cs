using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerService.WebApi.CommandRequests;

public sealed record AddStartPointCommand(int TaskId, string StartPoint) : ICommand<TaskStartPointDto>;
