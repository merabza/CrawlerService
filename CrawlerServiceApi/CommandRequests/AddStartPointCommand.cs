using CrawlerServiceShared.Contracts;
using SystemTools.Application.Abstractions.Messaging;

namespace CrawlerServiceApi.CommandRequests;

public sealed record AddStartPointCommand(int TaskId, string StartPoint) : ICommand<TaskStartPointDto>;
