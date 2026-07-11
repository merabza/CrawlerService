using CrawlerServiceShared.Contracts;
using SystemTools.MediatRMessagingAbstractions;

namespace CrawlerServiceApi.CommandRequests;

public sealed record AddStartPointCommand(int TaskId, string StartPoint) : ICommand<TaskStartPointDto>;